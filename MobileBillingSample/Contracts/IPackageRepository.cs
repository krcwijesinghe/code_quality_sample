using System;
namespace MobileBillingSample
{
    /// <summary>
    /// Contract for a mobile Package repository
    /// </summary>
    public interface IPackageRepository
    {
        Package GetPackage(string packageCode);
    }
}
