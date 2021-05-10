: Generates UML diagram as a .svg file and opens it.
: 
: DEPENCIES:
: https://github.com/pierre3/PlantUmlClassDiagramGenerator
: Has to be installed locally. Follow the instructions in the repository.
: https://plantuml.com/
: MIT-licensed version is included, but you might need things like Java.
@echo off
puml-gen ./WormGame/ ./Documentation/Generated/ -dir -createAssociation -allInOne > nul
java -jar ./Tools/PlantUML/plantuml.jar ./Documentation/Generated/include.puml -tsvg
start ./Documentation/Generated/include.svg