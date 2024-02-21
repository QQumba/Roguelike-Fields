## Interactions
The idea is to move player interactions in a separate components.

To be interactable with player cell no longer need to have 
enemy/pickable/activatable components. These components can have
a form of interactions that will define minimum components set required
for interaction and a player movement after interaction, e.g:
- stay in place
- move to cell
- swap with cell
- etc.
