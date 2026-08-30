# Task 1 fix round 1 review package

Non-Git project. Fix base is the first reviewed test file; head adds only the following assertions inside `CorridorAndLibrary_ConfigureMatchingBidirectionalDoorTransitions` after `AssertDoor`:

```diff
 AssertDoor(corridor, "Library Door", "SchoolLibrary", "LibraryFromCorridor");
+GameObject libraryDoor = Find(corridor, "Library Door");
+Assert.That(libraryDoor.transform.position,
+    Is.EqualTo(new Vector3(27f, 2.5f, 0f)), "Library Door");
+Assert.That(Find(corridor, "Decorative Door 1"), Is.Null,
+    "Decorative Door 1");
 AssertSpawn(corridor, "CorridorFromLibrary", new Vector2(27f, 1.1f),
```

No other code or asset changed in this fix round. Covering result: `Logs/school-library-red-fix1.xml`, 3 tests, 0 passed, 3 expected failures, no compile errors.
