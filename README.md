Phantom
=======

*Phantom* is a Unity project that was made for live visuals for [Phantom Sketch Mod.][PSM] held in Sapporo on 15th Oct. 2016. Some photos and videos from the event are available [here][Album].

![screenshot](http://66.media.tumblr.com/30e0037ada2dd6d07ad6d25dfd220895/tumblr_of1bbsGMIj1qio469o3_320.png)
![gif](http://67.media.tumblr.com/cc33e37b3a67afdb543914a271a42e44/tumblr_oeu4kzqGEg1qio469o2_320.gif)
![gif](http://66.media.tumblr.com/b7d15c5b33b082a02a53cb3a73aa9b4e/tumblr_oern2pN2kX1qio469o1_320.gif)
![gif](http://67.media.tumblr.com/d4d3aef753c4c23c2cfd67c289d178d0/tumblr_oeq89sV7WN1qio469o1_320.gif)

*Phantom* was made with Unity 5.4.1. Although it's compatible with most of the desktop-class platforms (Windows, Mac, Linux, PS4, Xbox One, etc.), it's using [the multi-display feature][MultiDisplay] that is only stable on Windows at the moment. That feature is used to provide the VJing UI -- it shows the UI and previews on the primary display (monitor screen) and sends renders without UI to the secondary display (projector screen).

*Phantom* uses some GPU intensive image effects (e.g. SSAO, DOF, MotionBlur). In the event, a desktop PC with GeForce GTX 1070 was used to keep flawless 60fps on 1080p. If you're trying to run it on a less powerful PC, you'd have to turn off some of the effects to reduce GPU load.

License
-------

*Phantom* and its source code was released under the MIT license.

I don't declare any copyright on visuals made with *Phantom*. I mean, you can use it for VJing freely.

[PSM]: https://no-maps.jp/event/2016_int_psm
[Album]: http://radiumsoftware.tumblr.com/tagged/phantom
[MultiDisplay]: https://docs.unity3d.com/Manual/MultiDisplay.html
