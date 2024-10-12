CLASS STRUCTURE
---------------

### Space
- Position_Range (x,y,z)
- scopes[ ] -- *list for scopes in the space*
- objects[ ] -- *list for objects in the space*
- subjects[ ]  -- *list for subjects in the space*
### Environment
- super(space) *inherit from parent*
- seed[ ] --  *a long integer to generate landscape*
- action_note[ ] --  *a list of changes that subjects did*
- map[ ] --  *a 3D list to note the details subjects and objects of environment*
### Inerpret Space
- super(space)
- additional_scopes[ ]
- additional_objects[ ]
- additional_subjects[ ]
### Principle
- action_source -- *subject or space as the source of action trig*  
- action_range -- *the range of action trig in environment*  
- action_time_limit -- *the time limitation for the action happen*  
- object_source -- *some objects that need for the action*  
- effect_list[] -- *list of all subjects/objects can be effected and the details of changes on attributes, states*

### Object
- UID
- Type
- attribute_list[]
### Material -- *must include when environment generated*
- super(object)
- value_coefficient -- *measure the possibility of amount in environment*
### Craft
- super(object)
- manufacture_formula:[ ] -- *a 2D or 3D list of how to craft*
- manufacture_tool:[ ] -- *the craft/subject that can achieve the craff*
### Subject
- UID
- Type
- Name
- storage_ability
- storage[ ]
- attribute_list[ ]
- object_can_generate
- object_gene_coefficient
### NPC
- super()
- action -- *how npc move like game AI stuff*
### Agent
- super()
- actions *actions they can do*
