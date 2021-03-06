﻿using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    // TODO: doc
    public static unsafe class DeviceMemoryExtensions
    {
        /// <summary>
        /// Get a Windows HANDLE for a memory object.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static IntPtr GetWin32HandleKhx(this DeviceMemory memory, ExternalMemoryHandleTypesKhx handleType)
        {
            IntPtr handle;
            Result result = vkGetMemoryWin32HandleKHX(memory.Parent, memory, handleType, &handle);
            VulkanException.ThrowForInvalidResult(result);
            return handle;
        }

        /// <summary>
        /// Get a POSIX file descriptor for a memory object.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="handleType"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static int GetFdKhx(this DeviceMemory memory, ExternalMemoryHandleTypesKhx handleType)
        {
            int fd;
            Result result = vkGetMemoryFdKHX(memory.Parent, memory, handleType, &fd);
            VulkanException.ThrowForInvalidResult(result);
            return fd;
        }

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetMemoryWin32HandleKHX(IntPtr device, long memory, ExternalMemoryHandleTypesKhx handleType, IntPtr* handle);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetMemoryFdKHX(IntPtr device, long memory, ExternalMemoryHandleTypesKhx handleType, int* fd);
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryAllocateFlagsInfoKhx
    {
        public StructureType Type;
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public MemoryAllocateFlagsKhx Flags;
        public int DeviceMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryAllocateFlagsInfoKhx"/> structure.
        /// </summary>
        /// <param name="flags">Pointer to next structure.</param>
        /// <param name="deviceMask"></param>
        /// <param name="next"></param>
        public MemoryAllocateFlagsInfoKhx(MemoryAllocateFlagsKhx flags, int deviceMask, IntPtr next = default(IntPtr))
        {
            Type = StructureType.MemoryAllocateFlagsInfoKhx;
            Next = next;
            Flags = flags;
            DeviceMask = deviceMask;
        }
    }

    /// <summary>
    /// Bitmask specifying flags for a device memory allocation.
    /// </summary>
    [Flags]
    public enum MemoryAllocateFlagsKhx
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// Force allocation on specific devices.
        /// </summary>
        MemoryAllocateDeviceMaskKhx = 1 << 0
    }

    /// <summary>
    /// Specify that an image will be bound to swapchain memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageSwapchainCreateInfoKhx
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Is 0 or a handle of an <see cref="SwapchainKhr"/> that the image will be bound to.
        /// </summary>
        public long Swapchain;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSwapchainCreateInfoKhx"/> structure.
        /// </summary>
        /// <param name="swapchain">
        /// Is <c>null</c> or a handle of an <see cref="SwapchainKhr"/> that the image will be bound to.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public ImageSwapchainCreateInfoKhx(SwapchainKhr swapchain, IntPtr next = default(IntPtr))
        {
            Type = StructureType.ImageSwapchainCreateInfoKhx;
            Next = next;
            Swapchain = swapchain;
        }
    }

    /// <summary>
    /// Structure indicating which instances are bound.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceGroupBindSparseInfoKhx
    {
        /// <summary>
        /// The type of this structure.
        /// </summary>
        public StructureType Type;
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A device index indicating which instance of the resource is bound.
        /// </summary>
        public int ResourceDeviceIndex;
        /// <summary>
        /// A device index indicating which instance of the memory the resource instance is bound to.
        /// </summary>
        public int MemoryDeviceIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGroupBindSparseInfoKhx"/> structure.
        /// </summary>
        /// <param name="resourceDeviceIndex">
        /// A device index indicating which instance of the resource is bound.
        /// </param>
        /// <param name="memoryDeviceIndex">
        /// A device index indicating which instance of the memory the resource instance is bound to.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public DeviceGroupBindSparseInfoKhx(int resourceDeviceIndex, int memoryDeviceIndex,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.DeviceGroupBindSparseInfoKhx;
            Next = next;
            ResourceDeviceIndex = resourceDeviceIndex;
            MemoryDeviceIndex = memoryDeviceIndex;
        }
    }
}
