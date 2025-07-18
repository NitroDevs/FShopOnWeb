module CentralPackageManagementTests

open Xunit
open System.IO

[<Fact>]
let ``Central Package Management - Directory.Packages.props exists`` () =
    // This test verifies that Central Package Management is properly configured
    let solutionRoot = Path.Combine(__SOURCE_DIRECTORY__, "..")
    let centralPkgFile = Path.Combine(solutionRoot, "Directory.Packages.props")
    Assert.True(File.Exists(centralPkgFile), "Directory.Packages.props should exist for Central Package Management")
    
[<Fact>]
let ``Central Package Management - Directory.Packages.props contains ManagePackageVersionsCentrally`` () =
    // Verify the central package management is enabled
    let solutionRoot = Path.Combine(__SOURCE_DIRECTORY__, "..")
    let centralPkgFile = Path.Combine(solutionRoot, "Directory.Packages.props")
    let content = File.ReadAllText(centralPkgFile)
    Assert.Contains("ManagePackageVersionsCentrally", content)
    Assert.Contains("true", content)

[<Fact>]
let ``Central Package Management - Project files do not contain Version attributes`` () =
    // Verify project files use central package management (no version attributes)
    let solutionRoot = Path.Combine(__SOURCE_DIRECTORY__, "..")
    let webProject = Path.Combine(solutionRoot, "src", "Microsoft.eShopWeb.Web", "Microsoft.eShopWeb.Web.fsproj")
    let testProject = Path.Combine(solutionRoot, "tests", "FShopOnWeb.Tests.fsproj")
    
    let webContent = File.ReadAllText(webProject)
    let testContent = File.ReadAllText(testProject)
    
    // Should not contain Version attributes in PackageReference elements
    Assert.DoesNotContain("PackageReference Include=\"EntityFrameworkCore.FSharp\" Version=", webContent)
    Assert.DoesNotContain("PackageReference Include=\"Falco\" Version=", webContent)
    Assert.DoesNotContain("PackageReference Include=\"xunit\" Version=", testContent)