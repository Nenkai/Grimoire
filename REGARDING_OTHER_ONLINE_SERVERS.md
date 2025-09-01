# Regarding "Other" Online Servers

This repository is the first and only implementation of a custom server for either GT5, or GT6.

I've been aware that other projects have been using this very source **out of my will**, such as projects run by a certain 'Irekej'.

I initially released a proof of concept (in the event that I am unable to release anything, just incase), while working on an actual server.

This proof-of-concept, is, well, a PoC, and absolutely not suitable for wide usage. Why? There is no security measures whatsoever:

* **IPs can be grabbed**
* MITM (man-in-the-middle) possible as there is no HTTPS/SSL encryption, forgery is possible so is DoS (denial of service)
* No NP ticket authentication whatsoever, **you can pretend to be other users**
* **No security on the database itself** (likely breaches a few laws out there)

Most importantly **it still requires PSN and a jailbroken console**, which is a big no-no. It's essentially hijacking sony's servers and playing with other users on hacked hardware, meaning your account can be banned at any given time, whether by detection, or by other user reports.

I tried to contact the owner of this project for a compromise (for instance, using RPCS3's RPCN, or using the game's built-in no-PSN features), but I was faced with constant childish remarks, **little credit**, and they simply pretended to agree in taking proper actions to host such a project.

---

This person in particular was well aware that I was working on my own and I have kindly asked not to host theirs using this proof-of-concept to avoid any potential problems with Sony. They could in fact, revoke the NP commnunication key for either games at any given time, which would severly hamper any  reverse-engineering efforts (they have no reverse-engineering knowledge). Unfortunately they still proceeded and I've seen them first hand host servers with their friends, and then it grew out of proportion into an actual 'project'.

This whole deal has been fueled by what seems to be a lack of patience from the south-american Gran Turismo community which was blindly used to ensure some level of presence and advertisement. To this day I am still seeing this project advertised without any credit whatsoever. Any broken or unimplemented feature that you might see on there are solely because no one knows how to get them working.

The actual server I was working on was in fact, for the most part, complete (yes, including [Gran Turismo TV](https://www.youtube.com/watch?v=CR6LR0b2_ZE&t=79s), Online BSpec, all community features, GT6 Quickmatching, Clubs, and more). I've put it aside due to a lack of motivation from this incident. 

**Until a project can be created in a responsible manner, this repository (and my other, actual server), will remain this way.**

[Some conversations here.](https://imgur.com/a/xl9qgpK)
