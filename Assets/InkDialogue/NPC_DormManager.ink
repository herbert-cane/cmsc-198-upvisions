=== dorm_manager ===
Male or Female wing?
We are at 90% capacity. We prioritize students based on the Point System. Where is your permanent residence?
    * [Far] Luzon / Mindanao / Outside Panay.
        -> far_residence
    * [Mid-Range] Aklan / Capiz / Antique (3-4 hours away).
        -> mid_residence
    * [Near] Iloilo City / Oton (1-2 hours away).
        -> near_residence

=== far_residence ===
Okay, that gives you maximum points for Distance. Income bracket?
Alright. You qualify for a slot. Here are the rules:
1. Curfew is 10:00 PM. The gates are locked.
2. No electric coils, heaters, or rice cookers inside the room.
3. Visitors are allowed in the lobby only.
    * [Understood] Understood.
        -> sign_dorm
    * [Curious] What happens if I miss curfew?
        -> curfew_consequences

=== mid_residence ===
Okay, that gives you good points for Distance. Income bracket?
Alright. You qualify for a slot. Here are the rules:
1. Curfew is 10:00 PM. The gates are locked.
2. No electric coils, heaters, or rice cookers inside the room.
3. Visitors are allowed in the lobby only.
    * [Understood] Understood.
        -> sign_dorm
    * [Curious] What happens if I miss curfew?
        -> curfew_consequences

=== near_residence ===
You live in the city? You are currently Waitlist #5. You might need to look for a boarding house in town if a slot doesn't open up by Friday.
-> END

=== curfew_consequences ===
First offense, warning. Second offense, community service. Third offense, eviction. Don't test me. Sign here. Your room key is #ROOM_NUMBER.
-> sign_dorm

=== sign_dorm ===
First offense, warning. Second offense, community service. Third offense, eviction. Don't test me. Sign here. Your room key is #ROOM_NUMBER.
-> END
