### CPTS 487 – Special Feature A  
### “Screen Flip Gimmick”  
### Nexus Horizon  

# Secret Feature Design Plan: Generic Screen‑Flip Gimmick

## Overview  
We want a “flip” system that can mirror or rotate the entire play area on demand.  
**Flip modes**  
- **Horizontal**: mirror left ↔ right  
- **Vertical**: mirror top ↔ bottom  
- **UpsideDown**: rotate 180°  
- **FaceRight**: rotate 90° clockwise  
- **FaceLeft**: rotate 90° counter‑clockwise

---

## Components and changes

| **File / Class**    | **Role**                       | **Changes / Additions**                                                                                                                         |
|---------------------|--------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------|
| **FlipManager**     | flip controller                | • New class                                                                                                                                     |
|                     |                                | • Public:                                                                                                                                       |
|                     |                                |   – `StartFlip(durationSeconds, FlipType)`                                                                                                      | 
|                     |                                |   – `Update(GameTime)`                                                                                                                          |
|                     |                                |   – `Matrix GetTransform()`                                                                                                                     |
| **GameM**           | Application bootstrap & loop   | • In `LoadContent()`, call `FlipManager.Instance.Init()`                                                                                        |
|                     |                                | • In `Update()`, call `FlipManager.Instance.Update(gameTime)`                                                                                   |
| **Renderer**        | Rendering                      | • In `BeginRender()`, change/add `spriteBatch.Begin(transformMatrix: FlipManager.Instance.GetTransform(), …)`                                   |
| **CallerTypes**     | EnemyState, Input handler, etc.| • Invoke `FlipManager.Instance.StartFlip(5f, FlipManager.FlipType.Horizontal)` (or other type) where you want the flip to occur                 |
| **OtherClasses**    | Unchanged                      | • No changes needed for flip effect                                                                                                             |

---

## How It Works 

 **StartFlip**  
   - You request a flip for X amount of seconds.  
   - `FlipManager` stores the end time and flip type.

 **Update**  
   - Each frame, `FlipManager.Update(gameTime)` checks if the flip is still active.  
   - When the time is up, it resets to “no flip.”

**GetTransform**  
   - Returns an identity matrix if no flip.  
   - Otherwise returns one of the five matrices (horizontal mirror, vertical mirror, rotate 90° both ways, upside down)

**Rendering**  
   - `Renderer.BeginRender` uses the matrix from `GetTransform`.  
   - All drawing commands after that automatically flip the screen.

---

## Where to Use

- **Boss fights**: for example, after 10 s, flip for 5 s, then back.
- **AnywhereDesired**: any code can call `StartFlip(...)`

Can use `TimerContainer` + `DelayTimer` or call `StartFlip` and let `FlipManager` handle it.

- **Renderer** needs to be updated in begin render 
- **Game** constantly updated (initialization and instance)

---

## Design Notes

- only need one `FlipManager.Instance`

- **Separation**:  
  - `FlipManager` only tracks timing & flip type.  
  - `Renderer` only applies the transform.  
  - Game code only calls `StartFlip`.

  - **Strategy**: matrix selection in GetTransform()
  - **Timer**: scheduling in game logic 

- **Extension Options**:  
  - To add a new flip like if we wanted diagonal, just add an entry and matrix in `GetTransform()`.  
  - No other code is needed to change.

---

## Data 

**We can add things like this**

public enum FlipType 
{
  None,
  Horizontal,
  Vertical,
  UpsideDown,
  FaceRight,
  FaceLeft
}

// helper for storing current flip
internal struct FlipState
{
  public FlipType Type;    // which flip is active
  public double EndTime;    // when to stop flipping (in seconds)
}

**In Renderer**
public static void BeginRender()
 {
-    spriteBatch.Begin(…);
+    spriteBatch.Begin(
+        transformMatrix: FlipManager.Instance.GetTransform(),
+        sortMode: SpriteSortMode.Deferred,
+        samplerState: SamplerState.PointClamp);
 }

**In Game**
 protected override void LoadContent()
 {
     … 
+    FlipManager.Instance.Init();
 }

---

## Input 
- Need to transform mouse coordinates via inverse of `GetTransform()`

---

## Performance and edge cases
- compute flip matrices
- duration ≤ 0 → ignore
- multiple calls override


