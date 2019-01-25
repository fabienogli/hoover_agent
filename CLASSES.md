Classes : 

- Environnement 
 - Tableaux de 100 cases (either enum, or bits) avec les valeurs {0, 1, 2}, avec 0 = Rien, 1 = Poussière, 2 = Bijou
    => Si jamais on peut avoir les deux, use [Flags] on Enum (bitwise operation/bitmasks => efficient test) 
        =>On set avec  "Poussière | Bijou"
        =>On test avec "flags & Bijou", true si y a un bijou 
 - Indicateur de performance ( reward/fitness )
    - Chaque action impacte la performance
    - Si l'agent aspire de la poussière => Good
    - Si l'agent aspire un bijou => Very bad
    - Si l'agent ramasse poussière/rien => bad
    - Si l'agent ramasse un bijou => Good (maybe very good(but less than "aspire un bijou"))
    - Déplacement = Ok (0 impact probablement, peut être un léger négatif, pour pas que l'agent se contente de tourner en rond)
 - Fonction de successor 
    - A chaque état e, on assossie un ensemble de couple (action, etat suivant) 
    => successor(state) = [action1 => nextState1, action2 => nextState2]
    => Toujours les mêmes actions, mais pas les même state 
    => Nécéssaire pour une solution "générique" au problème d'exploration
 
 - Loop { //In a concurrent way
     If(shouldGenerateNewDirt()) { //Random test with proba
         generateDirt()
     }
     if(shouldGenerateJewel()) { //Random test with proba
         generateJewel()
     }
     //Sleep ? //Just to avoid CPU overload
 }

- Agent 
  - Sensor<T>(avec de la généricité ça peut être bien je pense)
    méthode T Capture(env) qui renvoit l'info (copie) correspondant au moment de la relève

    Du coup on aurait chez l'agent Sensor<Cases> + Sensor<Performance>
    
  - Enum d'action
    {
        RIEN (?),
        UP,
        DOWN,
        LEFT,
        RIGHT,
        SUCK,
        TAKE
    }

  - Effectors
    - env.RequestAction(action)

   - Mental state 
     Beliefs => Comment est l'env a ce moment la
     Desires => Est ce que mon but à changé ? (à priori non mais pour la modé il le faut)
     Itentions => Qu'elles actions je vais effectuer ? 

  - Loop { //In a concurrent way
    obs = sensors.observe();
    mindset.Beliefs.Update(obs);
    if( desire != null) {
        if (intents != null) { //Ajouter condition ici pour la contrainte 8
            action = intents[0]
            sensor.act(action)
        } else {
            intents = this.plan(states, desire) ==> La je sors ça du cours, states, je sais pas si c'est l'état courant, l'ensemble des états etc.., je vote pour état courant tho
        }
    } 
  }

- Tree (can be fully generic I belive)
  - Internal Class Node 
     - Depth
     - Parents (Singular ?)
     - Left child 
     - Right child 
     - State (Les cases du tableaux, copiés bien entendu, mais en mode générique (Cases extend State par exemple)) 
     - Action (L'action effectué pour arrivé dans l'état)
     - Cost
  - &Env  => Référence sur l'env/le problème

- Tree Search 
  -  Frontier (Iterator pattern)
    - Generic data structure => Interface
    - Next() -> Return the next node according to strategy (default will be FIFO)
    - HasNext() -> false if empty, true otherwise
    - InsertAll(nodes) -> Add all nodes to the structure (can be overriden to change data structure)
  Loop {
      if (!frontier.HasNext()) {
        FAIL (throw exception)
      }
      node = frontier.Next()
      successors = expand(Node)
      frontier.InsertAll(successors)
  }

  expand(node) {
      sucessors = []
      env.successors(node.GetState()).forEach(
          (action, result) => {
            child = new Node()
            s.parent = node
            s.action = action
            s.state = result
            s.cost = node.cost + cost(action)
            s.depth = node.depth + 1
            successors.add(s) 
          }
      )
      return successors
  }