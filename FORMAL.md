# Environnement Modélisation 
- Partiellement observable
- Stochastique 
- Séquentiel
- Dynamique
- Continue ?
- Un agent

Etat initial : 
- 100 pièces, avec génération aléatoire de poussière/bijoux (20%/20% me parait honnête)

Etat final : (pas atteignable car génération continue)
- 100 pièces, avec aucune poussière/bijoux 

Actions: 
- Aspirer (same pour poussière ET bijou)
- Ramasser bijou 
- Déplacement vers le haut
- Déplacement vers le bas
- Déplacement à droite
- Déplacement à gauche

Coût : 1 pour chaque action

Solution : Séquence d'action


# Modélisation Agent 
Capteurs : 
 - Mesure de performance
 - Capteur des pièces => Pour chaque pièce, qu'est ce que j'ai (copy state to ensure we don't have a reference to the current state)



Effecteurs : 
 - Déplacement HAUT
 - Déplacement BAS 
 - Déplacement GAUCHE
 - Déplacement DROIT 
 - Aspirateur
 - Ramasseur
