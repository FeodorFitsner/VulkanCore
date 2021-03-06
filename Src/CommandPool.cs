﻿using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a command pool object.
    /// <para>
    /// Command pools are opaque objects that command buffer memory is allocated from, and which
    /// allow the implementation to amortize the cost of resource creation across multiple command
    /// buffers. Command pools are application-synchronized, meaning that a command pool must not be
    /// used concurrently in multiple threads. That includes use via recording commands on any
    /// command buffers allocated from the pool, as well as operations that allocate, free, and reset
    /// command buffers or the pool itself.
    /// </para>
    /// </summary>
    public unsafe class CommandPool : DisposableHandle<long>
    {
        internal CommandPool(Device parent, CommandPoolCreateInfo* createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            createInfo->Prepare();
            long handle;
            Result result = vkCreateCommandPool(Parent, createInfo, NativeAllocator, &handle);
            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Gets the parent of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Reset a command pool.
        /// </summary>
        /// <param name="flags">
        /// Contains additional flags controlling the behavior of the reset. If flags includes <see
        /// cref="CommandPoolResetFlags.ReleaseResources"/>, resetting a command pool recycles all of
        /// the resources from the command pool back to the system.
        /// </param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public void Reset(CommandPoolResetFlags flags = 0)
        {
            Result result = vkResetCommandPool(Parent, this, flags);
            VulkanException.ThrowForInvalidResult(result);
        }

        /// <summary>
        /// Allocate command buffers from an existing command pool.
        /// </summary>
        /// <param name="allocateInfo">The structure describing parameters of the allocation.</param>
        /// <returns>
        /// The resulting command buffer objects returned. Each allocated command buffer begins in
        /// the initial state.
        /// </returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public CommandBuffer[] AllocateBuffers(CommandBufferAllocateInfo allocateInfo)
        {
            return CommandBuffer.Allocate(this, &allocateInfo);
        }

        /// <summary>
        /// Free command buffers.
        /// </summary>
        /// <param name="commandBuffers">Command buffers to free.</param>
        public void FreeBuffers(params CommandBuffer[] commandBuffers)
        {
            CommandBuffer.Free(this, commandBuffers);
        }

        /// <summary>
        /// Destroy a command pool object.
        /// <para>
        /// When a pool is destroyed, all command buffers allocated from the pool are implicitly
        /// freed and become invalid. Command buffers allocated from a given pool do not need to be
        /// freed before destroying that command pool.
        /// </para>
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyCommandPool(Parent, this, NativeAllocator);
            base.Dispose();
        }
        
        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkCreateCommandPool(IntPtr device, 
            CommandPoolCreateInfo* createInfo, AllocationCallbacks.Native* allocator, long* commandPool);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkDestroyCommandPool(IntPtr device, long commandPool, AllocationCallbacks.Native* allocator);
        
        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkResetCommandPool(IntPtr device, long commandPool, CommandPoolResetFlags flags);
    }

    /// <summary>
    /// Structure specifying parameters of a newly created command pool.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CommandPoolCreateInfo
    {
        internal StructureType Type;
        internal IntPtr Next;

        /// <summary>
        /// A bitmask indicating usage behavior for the pool and command buffers allocated from it.
        /// </summary>
        public CommandPoolCreateFlags Flags;
        /// <summary>
        /// Designates a queue family. All command buffers allocated from this command pool must be
        /// submitted on queues from the same queue family.
        /// </summary>
        public int QueueFamilyIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandPoolCreateInfo"/> structure.
        /// </summary>
        /// <param name="queueFamilyIndex">
        /// Designates a queue family. All command buffers allocated from this command pool must be
        /// submitted on queues from the same queue family.
        /// </param>
        /// <param name="flags">
        /// A bitmask indicating usage behavior for the pool and command buffers allocated from it.
        /// </param>
        public CommandPoolCreateInfo(int queueFamilyIndex, CommandPoolCreateFlags flags = 0)
        {
            Type = StructureType.CommandPoolCreateInfo;
            Next = IntPtr.Zero;
            Flags = flags;
            QueueFamilyIndex = queueFamilyIndex;
        }

        internal void Prepare()
        {
            Type = StructureType.CommandPoolCreateInfo;
        }
    }

    /// <summary>
    /// Bitmask specifying usage behavior for a command pool.
    /// </summary>
    [Flags]
    public enum CommandPoolCreateFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Command buffers have a short lifetime.
        /// </summary>
        Transient = 1 << 0,
        /// <summary>
        /// Command buffers may release their memory individually.
        /// </summary>
        ResetCommandBuffer = 1 << 1
    }

    /// <summary>
    /// Bitmask controlling behavior of a command pool reset.
    /// </summary>
    [Flags]
    public enum CommandPoolResetFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Release resources owned by the pool.
        /// </summary>
        ReleaseResources = 1 << 0
    }
}
