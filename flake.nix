{
  description = "A basic C# development environment with .NET SDK";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
  };

  outputs = { self, nixpkgs, ... }:
  let
    system = "x86_64-linux";
    pkgs = import nixpkgs { inherit system; };
  in
  {
    devShells.${system}.default = pkgs.mkShell {
      buildInputs = [
        pkgs.dotnet-sdk_8
      ];
      shellHook = ''
        echo "C# development environment with .NET SDK 8"
        echo "Run 'dotnet new console -o HelloWorld' to create a new project"
        echo "Run 'dotnet run' inside the project directory to execute"
      '';
    };
  };
}