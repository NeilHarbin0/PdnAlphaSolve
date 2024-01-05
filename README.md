# AlphaSolve ![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/AlphaSolveIcon.png?raw=true)

This is a plugin for [Paint .NET](getpaint.net) to take two images of a subject, one with a black background, and one whith a white background, solving for the proper partial transparency and clean outline of the subject. Originally developed for use with Super Smash Brothers Melee debug mode to get clear images of character hit/hurtboxes.

## Usage
Once the plugin dll has been installed to the Paint .NET ```Effects``` folder and the application has been restarted, it can be found via the ```Effects -> Object``` sub-menu.

The target layer should contain one of the black or white background images, while the clipboard ```Ctrl + C``` should contain the opposite.

## Example

These two source images are used to solve for the proper transparency of the subject.

![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/Resources/SourceWhite.png?raw=true)
![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/Resources/SourceBlack.png?raw=true)

The plugin can produce either an alpha map, or the fully transparent subject.

![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/Resources/ExampleAlphaMap.png?raw=true)
![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/Resources/ExampleTransparent.png?raw=true)

## Other "Worse" Approaches
Below are images of other approaches which generally miss partial transparency, leading to the inability to place your transparent image onto any background with accurate color. They also tend to break down at low resolutions around the edges.

### Greenscreen

![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/Resources/ExampleGreenscreen.png?raw=true)
- Green edge glow
- Innacurate or missing partial transparency


### Magic Wand

Only of these images may look relatively fine depending on if you view GitHub in light or dark mode.

![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/Resources/ExampleWandBlack.png?raw=true)
![Source Black](https://github.com/NeilHarbin0/PdnAlphaSolve/blob/master/AlphaSolve/Resources/ExampleWandWhite.png?raw=true)

- Innacurate and sometimes jagged edges
- No partial transparency
