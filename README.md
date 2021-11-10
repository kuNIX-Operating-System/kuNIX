# kuNIX

## ATTENTION: THE CURRENT OFFICIAL COMPILED VERSION WON'T WORK AS THERE IS A BUG IN THE INSTALLATION PROCESS. THIS BUG HAS BEEN PATCHED IN `kernel.cs`. FEEL FREE TO RECOMPILE ON YOUR OWN.

#### Links
[Official Site](http://kunix.rf.gd)

[Compiled](https://www.mediafire.com/file/8avthllro8wv80u/kuNIX.iso/file)

------

Hello, thank you for using kuNIX. kuNIX is open source and
free to use. Developed by Scott and YoPoster, along with help
from gOS and SharpOS for some code. kuNIX is based on the
CosmosOS kernel, drivers and bootloader.


### For users:
Feel free to look through the source and edit whatever you like!

### For developers:
You can use the kuNIX kernel however you like. kuNIX has a GPL 3.0
license, meaning the software is free and open source. To make a
graphical version of kuNIX, consider using CGS (Cosmos Graphic
Subsystem) for making a GUI. We highly recommend that you heavily
modify this kernel for graphical use. Rather than typing characters,
you'd be using it graphically, so it'd make more sense to remove
Console text and inputs all together and have the GUI send data
to the kernel in a syscall.

### How to compile:
kuNIX can be compiled just like any other OS built on Cosmos.
Follow the instructions and make sure you configure Cosmos to use namespace "kunix".
