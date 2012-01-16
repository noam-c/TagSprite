TagSprite README
==================

TagSprite is a free and open-source tool used to create a metadata file that maps labels to the locations of sprites in a spritesheet. By using TagSprite, you can avoid the need to split a spritesheet into separate image files or create a specific ordering of sprite frames for all your sheets.

Many sprites are made available online in evenly spaced single-file sheets. It is generally optimal to handle spritesheet images instead of having each sprite frame in a different file due to the file loading and memory overhead. However, it is necessary for your game to know where each sprite is within the image so that it can display the right frame at the right time so that it can display the right frame at the right time. TagSprite's metadata files (those with the .eds extension) contain human-readable labels for each sprite in a given sheet.

Since sprite frames can have varying sizes, TagSprite requires that each sprite frame is surrounded by a 1-pixel border of transparency.

TagSprite is programmed using C# with WinForms. Although the .NET Framework and Visual Studio can be used to build and run TagSprite, it is fully compatible with the cross-platform Mono framework as well.

USAGE
=====

After running TagSprite, go to the File menu and select "Import Image..." (alternatively, press Ctrl+I). From here, select a spritesheet from your game or application. Once the spritesheet is imported, you will see a list of "untitled" frames that you can label. Selecting any frame will display a preview of the frame on the right side of the window. Go through each frame and label it as you desire. Note that for quick cycling between sprites, you may use the shortcut Ctrl+PgUp to go to the previous sprite, or Ctrl+PgDn to go to the next one.

After you are finished labeling the frames, you may go to the File menu and select "Save" (or press Ctrl+S) to create a .eds file next to your image. If you wish to edit the labels without starting over, simply load the metadata file using the File > "Open Sheet..." command (Ctrl+O).

LICENSE AND COPYRIGHT
=====================

Most of the code is Copyright 2012 Noam Chitayat. Contributors retain copyright to their original contributions.

The TagSprite source is released under the terms of the MIT license. For the full text of the MIT license in use, please refer to the LICENSE.txt file.

TagSprite references the Json.NET library. The Json.NET library is copyright James Newton-King. All rights reserved.

DEPENDENCIES
============

TagSprite requires the .NET Framework 4.0 or above or a compatible version of the Mono framework. TagSprite can be built using any IDE that reads C# .sln files, such as Visual Studio 2010 or MonoDevelop.

TagSprite requires the Json.NET library: http://json.codeplex.com
