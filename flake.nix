{
  description = "A basic C# development environment with .NET SDK";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
  };

  outputs = { self, nixpkgs, ... }:
  let
    forAllSystems = systems: f: nixpkgs.lib.genAttrs systems (system: f {
      pkgs = import nixpkgs { inherit system; };
    });
    systems = [ "x86_64-linux" "aarch64-darwin" "x86_64-darwin" ];
  in
  {
    devShells = forAllSystems systems ({ pkgs }: {
      default = pkgs.mkShell {
      buildInputs = [
        pkgs.dotnet-sdk_8
      ];
      shellHook = ''
        echo "C# development environment with .NET SDK 8"
        echo "Run 'dotnet new console -o HelloWorld' to create a new project"
        echo "Run 'dotnet run' inside the project directory to execute"
      '';
      };
    });
  };
}