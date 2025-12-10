=== physician ===
Name?
Any history of asthma? Heart conditions? Allergies? We are beside the ocean, and the pollen here is heavy.
    * [No allergies] No allergies, Doc.
        -> no_allergies
    * [Asthmatic] Actually, I have asthma.
        -> has_asthma
    * [Seasonal] Just seasonal allergies.
        -> seasonal_allergies

=== no_allergies ===
Okay. Blood pressure is normal. Height and weight recorded. Proceed to the Dental Section. Next room.
-> END

=== has_asthma ===
Okay. Blood pressure is normal. Height and weight recorded. Proceed to the Dental Section. Next room.
-> END

=== seasonal_allergies ===
Okay. Blood pressure is normal. Height and weight recorded. Proceed to the Dental Section. Next room.
-> END
