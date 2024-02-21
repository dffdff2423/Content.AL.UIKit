# AL UI Kit
[![](https://dcbadge.vercel.app/api/server/stVhj3dvCk)](https://discord.gg/stVhj3dvCk)

A collection of tools, widgets, etc for styling RobustToolbox games. A bit opinionated (see: BaseStyle requiring you specify two five color palettes) but only in ways you can simply ignore if you want.

**Last updated for RT version \[unreleased\]. May not work with newer builds!**<br/>
**Project is not yet versioned! Use at your own risk.**

## Adding to your project
This project is in the form of a **RobustToolbox mod** and is designed to be imported as a submodule, add it to your project's solution and use it as a dependency from your main client module (typically `Content.Client`). You may need to adjust your packaging script to make sure the additional dll is included with your client builds.
No resources are provided, but you'll want to have the following in your resources for the stylesheet defaults to work:
- NotoSans and NotoSans Display at `/Fonts/NotoSans/` and `/Fonts/NotoSansDisplay/` respectively. Make sure to include bold and italic variants.
  - As for why this doesn't just use the engine provided version, italic and bold are not included with the engine, as the font in the engine is intended to be replaced by the end user.
- NotoSans Symbols in `/Fonts/NotoSans/`.

If you're working on a certain established project about a space station, you'll already have these files in the VFS and you can ignore these.

## Usage
This control set is designed to be used in place of the engine builtins, so it is recommended that you use the `https://afterlight3149.net` xml namespace. Controls can be used as standard RTUI controls.
Documentation is heavily TODO, apologies!

## Showcase
![img.png](/readme_assets/img.png)
