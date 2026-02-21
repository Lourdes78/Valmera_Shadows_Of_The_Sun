# Architecture Snapshot 0.1

Sprint 0.1 – Core Architecture
Date: 19/02/2026

---

# SPRINT GOAL

Establir una arquitectura sòlida i escalable amb:

* Comunicació event-driven
* Estat persistent del jugador
* Sistema de nivell i XP
* Sistema de reputació
* Inventari i crafting base
* Save / Load funcional en JSON
* Model estàtic de zones (sense runtime encara)

Sprint 0.1 és exclusivament estructural.
No hi ha simulació global.
No hi ha WorldBrain encara.

---

# CORE SYSTEMS

## EventBus

* Tipat per events (genèric)
* Subscribe / Unsubscribe manual
* Publish centralitzat
* Logging opcional per debug
* No async
* Singleton via ServiceLocator

Responsabilitat:
Sistema central de comunicació desacoblada.
Cap sistema es comunica directament amb un altre.

---

## PlayerState

Responsabilitat:
Conté l'estat persistent del jugador.

Conté:

* XPSystem
* PlayerStats
* ReputationSystem
* InventorySystem
* CraftingSystem (async enabled)
* Influence (int)

No és MonoBehaviour.
És model pur.

Important:
PlayerState no guarda ni carrega directament.
Exposem mètodes per aplicar SaveData.

---

## PlayerStats

Conté:

* MaxHealth
* CurrentHealth
* MaxStamina
* CurrentStamina
* ToxicityResistance

Funcions:

* ModifyHealth()
* ModifyStamina()
* IncreaseMaxHealth()
* IncreaseMaxStamina()

Clamp via Math.Clamp.

Sistema completament independent del motor.

---

## XPSystem

Conté:

* Level actual
* CurrentXP
* Fórmula XP requerida

Funcionalitat:

* AddXP(int amount)
* Recalcula nivell
* Publica PlayerLevelUpEvent

No guarda estat per si mateix.
PlayerState gestiona persistència.

---

## ReputationSystem

Conté:

* Dictionary<Faction, int>

Funcions:

* AddReputation()
* SetReputation()
* GetAll()
* Clamp intern

Publica:

* ReputationChangedEvent

Preparat per:

* Gateig de missions
* Reacció NPC
* Restriccions de comerç

---

## InventorySystem

Característiques:

* Slots limitats (20)
* ItemDefinition (ScriptableObject)
* ItemInstance runtime
* Stackable logic
* Capacity limit

Publica:

* InventoryChangedEvent

Dissenyat per escalar a:

* Equipament
* Raresa
* Modificadors

---

## CraftingSystem

Característiques:

* Validació per CraftingStationType
* Async via Coroutine
* CraftTime per recepta
* Publish CraftingStartedEvent
* Publish CraftingCompletedEvent
* Remove ingredients AFTER delay
* Add result AFTER delay

Dependency:
Requereix MonoBehaviour runner injectat.

Sistema desacoblat del PlayerState excepte per inventari.

---

# SAVE SYSTEM (IMPLEMENTAT)

Responsabilitat:
Persistir estat runtime en JSON.

Conté:

* Save(PlayerState)
* Load() → retorna SaveData

SaveData inclou:

* Level
* CurrentXP
* Influence
* Reputation
* Inventari

Important:
SaveSystem NO modifica PlayerState directament.
Flux correcte:

1. SaveSystem.Load()
2. PlayerState.ApplyLoadedData(SaveData)

Això manté desacoblament i testabilitat.

Path:
Application.persistentDataPath/save.json

---

# WORLD STRUCTURE (STATIC ONLY)

## ZoneType (enum)

Defineix categories:

* Capital
* Village
* Wilderness
* Frontier
* Dungeon
* Ruins

---

## ZoneData (ScriptableObject)

Només definició estàtica.

Conté:

* ZoneId
* DisplayName
* ZoneType
* DefaultAlertLevel
* PoliticalWeight
* EconomyWeight

Important:
NO conté estat runtime.

Preparat per separar en Sprint 0.2:
ZoneRuntimeState gestionat pel WorldBrain.

---

# SERVICE LOCATOR

Registra:

* EventBus
* PlayerState
* SaveSystem

Inicialitzat a GameBootstrap.

Permet accés global controlat.

---

# INITIALIZATION FLOW

GameBootstrap:

1. Clear ServiceLocator
2. Crear EventBus
3. Crear PlayerState
4. Crear SaveSystem
5. Registrar serveis
6. Load Save
7. Aplicar dades a PlayerState

SceneContext:

* Subscriu a events
* Detecta LevelUp
* Executa SaveSystem.Save()

---

# EVENT FLOW EXEMPLE

Level Up:
XPSystem.AddXP()
→ PlayerLevelUpEvent
→ SceneContext
→ SaveSystem.Save(PlayerState)

Persistència immediata garantida.

---

# NO IMPLEMENTAT ENCARA

* WorldBrain
* ZoneRuntimeState
* Sistema de missions runtime
* NPC runtime
* Economia global
* SkillTree persistent
* Craft Queue
* Cancel system
* UI

---

# ARCHITECTURAL NOTES

* Core sense UI
* Event-driven
* Systems desacoblats
* Runtime separat de dades estàtiques
* Preparat per escalabilitat sistèmica
* Save/Load funcional i estable

Sprint 0.1 estableix els fonaments arquitectònics complets.

---

END SNAPSHOT 0.1