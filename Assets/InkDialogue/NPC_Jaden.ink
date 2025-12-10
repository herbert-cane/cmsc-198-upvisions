=== jaden ===
Hey! You survived Sir Ruel!
    * [Barely] Barely. He's intense.
        -> intense
    * [Easy] It was easy.
        -> easy
    * [Hungry] I need food. Now.
        -> hungry

=== intense ===
Same. I just came from the CUB too, got my partial stipend tagged. Hey, I'm heading to Lover's Lane to meet some friends. It's on the way to the dorms. Want to walk with me?
    * [Sure] Sure, let's go.
        -> lovers_lane_invite
    * [Where is it?] Where is Lover's Lane?
        -> explain_location

=== easy ===
Same. I just came from the CUB too, got my partial stipend tagged. Hey, I'm heading to Lover's Lane to meet some friends. It's on the way to the dorms. Want to walk with me?
    * [Sure] Sure, let's go.
        -> lovers_lane_invite
    * [Where is it?] Where is Lover's Lane?
        -> explain_location

=== hungry ===
Same. I just came from the CUB too, got my partial stipend tagged. Hey, I'm heading to Lover's Lane to meet some friends. It's on the way to the dorms. Want to walk with me?
    * [Sure] Sure, let's go.
        -> lovers_lane_invite
    * [Where is it?] Where is Lover's Lane?
        -> explain_location

=== explain_location ===
It's near HSU, after the dormitory area. Let's go there after you've finished talking with the dorm staff. It's nice and windy there. Come on, it's walking distance.
-> lovers_lane_invite

=== lovers_lane_invite ===
All settled?
Come on. Let's walk it off. My friends are waiting at the Lover's Lane.
They call this Lover's Lane.
    * [Dating] Why? Is it for dating?
        -> why_lovers_lane
    * [Jogging] Looks like a jogging path to me.
        -> why_lovers_lane

=== why_lovers_lane ===
Supposedly. But mostly, it's just where people go to breathe. When the exams pile up, or when you fail a Bluebook... you come here, look at the field, and realize the world is still big.
Classes start next week. You still have time to wander the premises.
    * [Ready] As ready as I'll ever be.
        -> ready
    * [Terrified] Terrified.
        -> terrified
    * [Let's do this] Let's do this.
        -> ready

=== ready ===
-> END

=== terrified ===
-> END
