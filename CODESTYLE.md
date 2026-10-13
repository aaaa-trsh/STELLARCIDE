# Code style

## Indentation, Spacing, and Braces
- Tab indentation, 4 spaces
- Newline braces for functions
  - Allman style
- Line length: use your best judgement
- Avoid doing too much in Monobehavior.Update(), use Observer/Listener pattern if possible

## Naming Conventions
- Class: PascalCase
  - PublicMember: PascalCase
  - privateMember: snakeCase
    - Exception: Monobehaviors
- Interfaces begin with I
- File: PascalCase
- Avoid abbreviations whenever possible
  - Exclusions: private RigidBody members, “obvious things” use your best judgement

## Math
- Spaces between operators and operands
- PEMDAS obviously

## Comment style
- Documentation comments for functions and classes ('///' should autocomplete)
- Don't need to comment everything, just stuff that you might expect someone else to use

## Language Standard
All code will be done in C#14 (the most up-to-date version)
