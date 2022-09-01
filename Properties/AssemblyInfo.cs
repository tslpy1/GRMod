using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(GRMod.BuildInfo.Description)]
[assembly: AssemblyDescription(GRMod.BuildInfo.Description)]
[assembly: AssemblyCompany(GRMod.BuildInfo.Company)]
[assembly: AssemblyProduct(GRMod.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + GRMod.BuildInfo.Author)]
[assembly: AssemblyTrademark(GRMod.BuildInfo.Company)]
[assembly: AssemblyVersion(GRMod.BuildInfo.Version)]
[assembly: AssemblyFileVersion(GRMod.BuildInfo.Version)]
[assembly: MelonInfo(typeof(GRMod.GRMod), GRMod.BuildInfo.Name, GRMod.BuildInfo.Version, GRMod.BuildInfo.Author, GRMod.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]