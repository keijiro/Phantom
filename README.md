Phantom
=======

*Phantom* is a Unity project that was made for live visuals for [Phantom Sketch Mod.][PSM] held in Sapporo on 15th Oct. 2016. Some photos and videos from the event are available [here][Album].

![screenshot](http://i.imgur.com/3BD3tAM.png)
![gif](http://i.imgur.com/U3GlUHH.gif)
![gif](http://i.imgur.com/9HjJKxb.gif)
![gif](http://i.imgur.com/YcQz9W5.gif)

*Phantom* was made with Unity 5.4.1. Although it's compatible with most of the desktop-class platforms (Windows, Mac, Linux, PS4, Xbox One, etc.), it's using [the multi-display feature][MultiDisplay] that is only stable on Windows at the moment. That feature is used to provide the VJing UI -- it shows the UI and previews on the primary display (monitor screen) and sends renders without UI to the secondary display (projector screen).

*Phantom* uses some GPU intensive image effects (e.g. SSAO, DOF, MotionBlur). In the event, a desktop PC with GeForce GTX 1070 was used to keep flawless 60fps on 1080p. If you're trying to run it on a less powerful PC, you'd have to turn off some of the effects to reduce GPU load.

License
-------

*Phantom* and its source code was released under the MIT license.

I don't declare any copyright on visuals made with *Phantom*. I mean, you can use it for VJing freely.

[PSM]: https://no-maps.jp/event/2016_int_psm
[Album]: http://radiumsoftware.tumblr.com/tagged/phantom
[MultiDisplay]: https://docs.unity3d.com/Manual/MultiDisplay.html
