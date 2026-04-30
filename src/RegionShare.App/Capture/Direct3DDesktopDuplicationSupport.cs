namespace RegionShare.App.Capture;

using Vortice.DXGI;
using Vortice.Direct3D;
using Vortice.Direct3D11;

public sealed class Direct3DDesktopDuplicationSupport : IScreenCaptureBackendSupport
{
    public bool IsDirect3DDesktopDuplicationSupported
    {
        get
        {
            try
            {
                using var factory = DXGI.CreateDXGIFactory1<IDXGIFactory1>();
                return TryGetAttachedOutput(factory);
            }
            catch
            {
                return false;
            }
        }
    }

    private static bool TryGetAttachedOutput(IDXGIFactory1 factory)
    {
        using var device = D3D11.D3D11CreateDevice(DriverType.Hardware, DeviceCreationFlags.BgraSupport);
        for (uint adapterIndex = 0; factory.EnumAdapters1(adapterIndex, out var adapter).Success; adapterIndex++)
        {
            using (adapter)
            {
                for (uint outputIndex = 0; adapter.EnumOutputs(outputIndex, out var output).Success; outputIndex++)
                {
                    using (output)
                    {
                        if (output.Description.AttachedToDesktop && CanDuplicateOutput(output, device))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private static bool CanDuplicateOutput(IDXGIOutput output, ID3D11Device device)
    {
        try
        {
            using var output1 = output.QueryInterface<IDXGIOutput1>();
            using var duplication = output1.DuplicateOutput(device);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
