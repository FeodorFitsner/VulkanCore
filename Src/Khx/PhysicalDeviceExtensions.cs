﻿using System;
using System.Runtime.InteropServices;
using VulkanCore.Khr;
using static VulkanCore.Constant;

namespace VulkanCore.Khx
{
    // TODO: doc
    public static unsafe class PhysicalDeviceExtensions
    {
        public static PhysicalDeviceProperties2Khx GetProperties2Khx(this PhysicalDevice physicalDevice)
        {
            PhysicalDeviceProperties2Khx.Native nativeProperties;
            vkGetPhysicalDeviceProperties2KHX(physicalDevice, &nativeProperties);
            PhysicalDeviceProperties2Khx.FromNative(ref nativeProperties, out var properties);
            return properties;
        }

        public static ImageFormatProperties2Khx GetImageFormatProperties2Khx(this PhysicalDevice physicalDevice,
            PhysicalDeviceImageFormatInfo2Khx info)
        {
            ImageFormatProperties2Khx properties;
            vkGetPhysicalDeviceImageFormatProperties2KHX(physicalDevice, &info, &properties);
            return properties;
        }

        public static ExternalBufferPropertiesKhx GetExternalBufferPropertiesKhx(this PhysicalDevice physicalDevice,
            PhysicalDeviceExternalBufferInfoKhx info)
        {
            ExternalBufferPropertiesKhx properties;
            vkGetPhysicalDeviceExternalBufferPropertiesKHX(physicalDevice, &info, &properties);
            return properties;
        }

        /// <summary>
        /// Function for querying external semaphore handle capabilities.
        /// </summary>
        /// <param name="physicalDevice"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static ExternalSemaphorePropertiesKhx GetExternalSemaphorePropertiesKhx(this PhysicalDevice physicalDevice,
            PhysicalDeviceExternalSemaphoreInfoKhx info)
        {
            ExternalSemaphorePropertiesKhx properties;
            vkGetPhysicalDeviceExternalSemaphorePropertiesKHX(physicalDevice, &info, &properties);
            return properties;
        }

        /// <summary>
        /// Query present rectangles for a surface on a physical device.
        /// </summary>
        /// <param name="physicalDevice"></param>
        /// <param name="surface"></param>
        /// <returns></returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static Rect2D[] GetPresentRectanglesKhx(this PhysicalDevice physicalDevice, SurfaceKhr surface)
        {
            int count;
            Result result = vkGetPhysicalDevicePresentRectanglesKHX(physicalDevice, surface, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var rectangles = new Rect2D[count];
            fixed (Rect2D* rectanglesPtr = rectangles)
            {
                result = vkGetPhysicalDevicePresentRectanglesKHX(physicalDevice, surface, &count, rectanglesPtr);
                VulkanException.ThrowForInvalidResult(result);
                return rectangles;
            }
        }

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceProperties2KHX(IntPtr physicalDevice, 
            PhysicalDeviceProperties2Khx.Native* properties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceImageFormatProperties2KHX(IntPtr physicalDevice, 
            PhysicalDeviceImageFormatInfo2Khx* imageFormatInfo, ImageFormatProperties2Khx* imageFormatProperties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceExternalBufferPropertiesKHX(IntPtr physicalDevice, 
            PhysicalDeviceExternalBufferInfoKhx* externalBufferInfo, ExternalBufferPropertiesKhx* externalBufferProperties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern void vkGetPhysicalDeviceExternalSemaphorePropertiesKHX(IntPtr physicalDevice, 
            PhysicalDeviceExternalSemaphoreInfoKhx* externalSemaphoreInfo, ExternalSemaphorePropertiesKhx* externalSemaphoreProperties);

        [DllImport(VulkanDll, CallingConvention = CallConv)]
        private static extern Result vkGetPhysicalDevicePresentRectanglesKHX(IntPtr physicalDevice, long surface, int* rectCount, Rect2D* rects);
    }

    public struct PhysicalDeviceProperties2Khx
    {
        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public PhysicalDeviceProperties Properties;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public PhysicalDeviceProperties.Native Properties;
        }

        internal static void FromNative(ref Native native, out PhysicalDeviceProperties2Khx managed)
        {
            managed.Next = native.Next;
            PhysicalDeviceProperties.FromNative(ref native.Properties, out managed.Properties);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceImageFormatInfo2Khx
    {
        internal StructureType StructureType;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public Format Format;
        public ImageType Type;
        public ImageTiling Tiling;
        public ImageUsages Usage;
        public ImageCreateFlags Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageFormatProperties2Khx
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        public ImageFormatProperties ImageFormatProperties;
    }

    /// <summary>
    /// Structure specifying buffer creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalBufferInfoKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask describing additional parameters of the buffer, corresponding to <see cref="BufferCreateInfo.Flags"/>.
        /// </summary>
        public BufferCreateFlags Flags;
        /// <summary>
        /// A bitmask describing the intended usage of the buffer, corresponding to <see cref="BufferCreateInfo.Usages"/>.
        /// </summary>
        public BufferUsages Usage;
        /// <summary>
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the buffer. See <see cref="ExternalMemoryHandleTypesKhx"/> for details.
        /// </summary>
        public ExternalMemoryHandleTypesKhx HandleType;
    }

    /// <summary>
    /// Structure specifying supported external handle capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalBufferPropertiesKHX
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// buffer creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhx ExternalMemoryProperties;
    }

    /// <summary>
    /// Structure specifying external memory handle type capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalMemoryPropertiesKhx
    {
        /// <summary>
        /// A bitmask describing the features of handle type. See <see
        /// cref="ExternalMemoryFeaturesKhx"/> below for a description of the possible bits.
        /// </summary>
        public ExternalMemoryFeaturesKhx ExternalMemoryFeatures;
        /// <summary>
        /// A bitmask specifying handle types that can be used to import objects from which handle
        /// type can be exported.
        /// </summary>
        public ExternalMemoryHandleTypesKhx ExportFromImportedHandleTypes;
        /// <summary>
        /// A bitmask specifying handle types which can be specified at the same time as handle type
        /// when creating an image compatible with external memory.
        /// </summary>
        public ExternalMemoryHandleTypesKhx CompatibleHandleTypes;
    }

    /// <summary>
    /// Bitmask specifying features of an external memory handle type.
    /// </summary>
    [Flags]
    public enum ExternalMemoryFeaturesKhx
    {
        /// <summary>
        /// Indicates that images or buffers created with the specified parameters and handle type
        /// must use the mechanisms defined in the "VKNVDedicatedAllocation" to create (or import) a
        /// dedicated allocation for the image or buffer.
        /// </summary>
        ExternalMemoryFeatureDedicatedOnly = 1 << 0,
        /// <summary>
        /// Indicates handles of this type can be exported from Vulkan memory objects.
        /// </summary>
        ExternalMemoryFeatureExportable = 1 << 1,
        /// <summary>
        /// Indicates handles of this type can be imported as Vulkan memory objects.
        /// </summary>
        ExternalMemoryFeatureImportable = 1 << 2
    }

    /// <summary>
    /// Structure specifying supported external handle capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalBufferPropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// buffer creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhx ExternalMemoryProperties;
    }

    /// <summary>
    /// Structure specifying semaphore creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalSemaphoreInfoKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bit indicating an external semaphore handle type for which capabilities will be returned.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhx HandleType;
    }

    /// <summary>
    /// Structure describing supported external semaphore handle features.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalSemaphorePropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// A bitmask specifying handle types that can be used to import objects from which
        /// handleType can be exported.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhx ExportFromImportedHandleTypes;
        /// <summary>
        /// A bitmask specifying handle types which can be specified at the same time as handleType
        /// when creating a semaphore.
        /// </summary>
        public ExternalSemaphoreHandleTypesKhx CompatibleHandleTypes;
        /// <summary>
        /// A bitmask describing the features of handle type.
        /// </summary>
        public ExternalSemaphoreFeaturesKhx ExternalSemaphoreFeatures;
    }

    /// <summary>
    /// Bitfield describing features of an external semaphore handle type.
    /// </summary>
    [Flags]
    public enum ExternalSemaphoreFeaturesKhx
    {
        /// <summary>
        /// Indicates handles of this type can be exported from Vulkan semaphore objects.
        /// </summary>
        Exportable = 1 << 0,
        /// <summary>
        /// Indicates handles of this type can be imported as Vulkan semaphore objects.
        /// </summary>
        Importable = 1 << 1
    }

    // TODO: Let Guids be marshalled directly?
    /// <summary>
    /// Structure specifying IDs related to the physical device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PhysicalDeviceIdPropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// An array of size <see cref="UuidSize"/>, containing 8-bit values that represent a
        /// universally unique identifier for the device.
        /// </summary>
        public fixed byte DeviceUUID[16];
        /// <summary>
        /// An array of size <see cref="UuidSize"/>, containing 8-bit values that represent a
        /// universally unique identifier for the driver build in use by the device.
        /// </summary>
        public fixed byte DriverUUID[16];
        /// <summary>
        /// A array of size <see cref="LuidSizeKhx"/>, containing 8-bit values that represent a
        /// locally unique identifier for the device.
        /// </summary>
        public fixed byte DeviceLUID[8];
        /// <summary>
        /// A boolean value that will be <c>true</c> if <see cref="DeviceLUID"/> contains a valid
        /// LUID, and <c>false</c> if it does not.
        /// </summary>
        public Bool DeviceLUIDValid;
    }

    /// <summary>
    /// Structure describing multiview limits that can be supported by an implementation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceMultiviewPropertiesKhx
    {
        internal StructureType Type;

        /// <summary>
        /// Pointer to next structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Max number of views in a subpass.
        /// </summary>
        public int MaxMultiviewViewCount;
        /// <summary>
        /// Max instance index for a draw in a multiview subpass.
        /// </summary>
        public int MaxMultiviewInstanceIndex;
    }

    /// <summary>
    /// Structure specifying supported external handle properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ExternalImageFormatPropertiesKHX
    {
        internal StructureType Type;

        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// Specifies various capabilities of the external handle type when used with the specified
        /// image creation parameters.
        /// </summary>
        public ExternalMemoryPropertiesKhx ExternalMemoryProperties;
    }

    /// <summary>
    /// Structure specifying external image creation parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PhysicalDeviceExternalImageFormatInfoKhx
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
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the image.
        /// </summary>
        public ExternalMemoryHandleTypesKhx HandleType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDeviceExternalImageFormatInfoKhx"/> structure.
        /// </summary>
        /// <param name="handleType">
        /// A bit indicating a memory handle type that will be used with the memory associated with
        /// the image.
        /// </param>
        /// <param name="next">
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </param>
        public PhysicalDeviceExternalImageFormatInfoKhx(ExternalMemoryHandleTypesKhx handleType,
            IntPtr next = default(IntPtr))
        {
            Type = StructureType.PhysicalDeviceExternalImageFormatInfoKhx;
            Next = next;
            HandleType = handleType;
        }
    }
}
