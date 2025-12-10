=== manong_driver ===
Sakay na! Infirmary (HSU)? Dorms?
I need to get my Medical Clearance first.
Ah, sa HSU. Medyo malayo 'yan. Nasa taas pa.
    * [Ride Tricycle (P15)] I'll take a ride. I don't want to be sweaty for the checkup.
        -> ride_tricycle
    * [Walk] I'll walk. Is it that far?
        -> walk_response

=== ride_tricycle ===
(The Player climbs into the sidecar. The tricycle roars past the gate, driving along the main avenue.)
You breeze past the New Administration Building on your left—it's surprisingly close to the gate. But the tricycle keeps going, climbing the road further into the campus.
Freshie? Saan galing?
    * [Local] Just from town/city.
        -> local_response
    * [Fish out of water] From outside Iloilo. I'm not used to this heat yet.
        -> outsider_response

=== local_response ===
That's nice. The Infirmary is busy today.
That's the "wet labs." If you smell fish later, that's normal.
-> END

=== outsider_response ===
Ah, dayo! Welcome. Iba ang init dito sa Miagao. Make sure you drink water. The Infirmary is busy today.
That's the "wet labs." If you smell fish later, that's normal.
-> END

=== walk_response ===
Bahala ka, 'nak. Walking distance ang New Admin, pero ang HSU? Good luck sa paahon.
-> END
