﻿module AsciiToSvg.SvgDocument

open System
open System.Globalization

let FontSize = 12.0
let FontOffsetWidth = 0.0
let FontOffsetHeight = 11.0
let FontFamily = "Consolas"
let GlyphWidth = 9.0
let GlyphHeight = 15.0
let GlyphWidthHalf = GlyphWidth / 2.0
let GlyphHeightHalf = GlyphHeight / 2.0
let CanvasWidth = 720.0   //  80 glyphs per line
let CanvasHeight = 675.0  // 45 lines
let Scale = { colsc = 1.0; rowsc = 1.0 }
let Fill = "black"
let Stroke = "black"
let StrokeWidth = 1.0

let culture = new CultureInfo("en-US")

let getOption (options: SvgOption) key defaultValue =
  if options.ContainsKey key then options.Item key else defaultValue

let getFloatOption (options: SvgOption) key (defaultValue:float) =
  if options.ContainsKey key then
    options.Item key
    |> function
    | Number x -> x
    | _ -> defaultValue
  else defaultValue

let getBooleanOption (options: SvgOption) key (defaultValue:bool) =
  if options.ContainsKey key then
    options.Item key
    |> function
    | Boolean x -> x
    | _ -> defaultValue
  else defaultValue

let getStringOption (options: SvgOption) key (defaultValue:string) =
  if options.ContainsKey key then
    options.Item key
    |> function
    | JString x -> x
    | _ -> defaultValue
  else defaultValue

let SvgTemplateOpen (options: SvgOption) =
  let canvasWidth = (getFloatOption options "canvas-width" CanvasWidth).ToString(culture)
  let canvasHeight = (getFloatOption options "canvas-height" CanvasHeight).ToString(culture)
  let fontSize = (getFloatOption options "canvas-font-size" FontSize).ToString(culture)
  "<?xml version=\"1.0\" standalone=\"no\"?>\n" +
  "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"" +
  "  \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">\n" +
  "<!-- Created with ASCIIToSVG (https://github.com/fbmnds/AsciiToSvg) -->\n" +
  (sprintf "<svg width=\"%spx\" height=\"%spx\" font-size=\"%s\" version=\"1.1\"\n" canvasWidth canvasHeight fontSize) +
  """
  xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">
  <defs>
    <filter id="dsFilterNoBlur" width="150%" height="150%">
      <feOffset result="offOut" in="SourceGraphic" dx="3" dy="3"/>
      <feColorMatrix result="matrixOut" in="offOut" type="matrix" values="0.2 0 0 0 0 0 0.2 0 0 0 0 0 0.2 0 0 0 0 0 1 0"/>
      <feBlend in="SourceGraphic" in2="matrixOut" mode="normal"/>
    </filter>
    <filter id="dsFilter" width="150%" height="150%">
      <feOffset result="offOut" in="SourceGraphic" dx="3" dy="3"/>
      <feColorMatrix result="matrixOut" in="offOut" type="matrix" values="0.2 0 0 0 0 0 0.2 0 0 0 0 0 0.2 0 0 0 0 0 1 0"/>
      <feGaussianBlur result="blurOut" in="matrixOut" stdDeviation="3"/>
      <feBlend in="SourceGraphic" in2="blurOut" mode="normal"/>
    </filter>
  </defs>
  """

let SvgTemplateClose = "  </svg>"

let ConvertCoordGridToSvg scale (gridCoord: GridCoordinates) =
  { colpx = ((float) gridCoord.col * GlyphWidth * scale.colsc) |> fun x -> Math.Round(x, 3)
    rowpx = ((float) gridCoord.row * GlyphHeight * scale.rowsc) |> fun x -> Math.Round(x, 3) }

let shiftColCoordGridToSvg scale coldpx (gridCoord: GridCoordinates) =
  ((float) gridCoord.col * GlyphWidth + coldpx) * scale.colsc |> fun x -> Math.Round(x, 3)

let shiftRowCoordGridToSvg scale rowdpx gridCoord =
  ((float) gridCoord.row * GlyphHeight + rowdpx) * scale.rowsc |> fun x -> Math.Round(x, 3)

let ShiftedCoordGridToSvg scale coldpx rowdpx (gridCoord: GridCoordinates) =
  { colpx = shiftColCoordGridToSvg scale coldpx gridCoord
    rowpx = shiftRowCoordGridToSvg scale rowdpx gridCoord }

let putTick (options: SvgOption) col1 row1 col2 row2 =
  [|"      <line "
    sprintf "stroke=\"%s\" " (getStringOption options "stroke" Stroke)
    sprintf "stroke-width=\"%.3f\" " (getFloatOption options "stroke-width" StrokeWidth)
    sprintf "x1=\"%.3f\" y1=\"%.3f\" x2=\"%.3f\" y2=\"%.3f\" />\n" col1 row1 col2 row2 |]
  |> Array.fold (fun r s -> r + s) ""

let putArc (options: SvgOption) col1 row1 col2 row2 col3 row3 =
  [|"      <path fill=\"none\" "
    sprintf "stroke=\"%s\" " (getStringOption options "stroke" Stroke)
    sprintf "stroke-width=\"%.3f\" " (getFloatOption options "stroke-width" StrokeWidth)
    sprintf "d=\"M %.3f %.3f Q %.3f %.3f  %.3f %.3f\" />\n" col1 row1 col2 row2 col3 row3 |]
  |> Array.fold (fun r s -> r + s) ""

let ScalableArrowTemplate (glyph: Glyph) (options: SvgOption) scale ax ay bx by cx cy dx dy ex ey =
  let cols = [|ax; bx; cx; dx; ex|] |> Array.map (fun x -> shiftColCoordGridToSvg scale x glyph.gridCoord)
  let rows = [|ay; by; cy; dy; ey|] |> Array.map (fun x -> shiftRowCoordGridToSvg scale x glyph.gridCoord)
  [|"      <polygon "
    sprintf "fill=\"%s\" " (getStringOption options "fill" Fill)
    sprintf "points=\"%.3f,%.3f " cols.[0] rows.[0]
    sprintf "%.3f,%.3f " cols.[1] rows.[1]
    sprintf "%.3f,%.3f " cols.[2] rows.[2]
    sprintf "%.3f,%.3f\" />\n" cols.[0] rows.[0]
    "      <line "
    sprintf "stroke=\"%s\" " (getStringOption options "stroke" Stroke)
    sprintf "stroke-width=\"%.3f\" " (getFloatOption options "stroke-width" StrokeWidth)
    sprintf "x1=\"%.3f\" y1=\"%.3f\" x2=\"%.3f\" y2=\"%.3f\" />\n" cols.[3] rows.[3] cols.[4] rows.[4] |]
  |> Array.fold (fun r s -> r + s) ""

let ArrowTemplate (glyph: Glyph) ax ay bx by cx cy dx dy ex ey =
  ScalableArrowTemplate glyph Map.empty Scale ax ay bx by cx cy dx dy ex ey

let ScalableCornerPath (glyph: Glyph) (options: SvgOption) scale =
  let cols = [|4.0; 4.0;  4.0; 0.0; 8.0|] |> Array.map (fun x -> shiftColCoordGridToSvg scale x glyph.gridCoord)
  let rows = [|7.0; 0.0; 14.0; 7.0; 7.0|] |> Array.map (fun x -> shiftRowCoordGridToSvg scale x glyph.gridCoord)
  match glyph.glyphKind with
  | UpperLeftCorner -> [|cols.[0]; rows.[0]; cols.[4]; rows.[4]; cols.[0]; rows.[0]; cols.[2]; rows.[2]|]
  | LowerLeftCorner ->  [|cols.[0]; rows.[0]; cols.[1]; rows.[1]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|]
  | UpperRightCorner -> [|cols.[0]; rows.[0]; cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[2]; rows.[2]|]
  | LowerRightCorner -> [|cols.[0]; rows.[0]; cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[1]; rows.[1]|]
  | UpperLeftAndRightCorner 
  | DownTick -> [|cols.[0]; rows.[0]; cols.[2]; rows.[2]; cols.[3]; rows.[3]; cols.[4]; rows.[4]|]
  | LowerLeftAndRightCorner 
  | UpTick -> [|cols.[0]; rows.[0]; cols.[1]; rows.[1]; cols.[3]; rows.[3]; cols.[4]; rows.[4]|]
  | UpperAndLowerRightCorner -> [|cols.[0]; rows.[0]; cols.[3]; rows.[3]; cols.[1]; rows.[1]; cols.[2]; rows.[2]|]
  | UpperAndLowerLeftCorner -> [|cols.[0]; rows.[0]; cols.[4]; rows.[4]; cols.[1]; rows.[1]; cols.[2]; rows.[2]|]
  | CrossCorner -> [|cols.[3]; rows.[3]; cols.[4]; rows.[4]; cols.[1]; rows.[1]; cols.[2]; rows.[2]|]
  | _ ->
#if DEBUG
    Log.logError "Wrong Glyph kind in ScalableCornerPath: %A" glyph 
#endif
    [|-1.0; -1.0; -1.0; -1.0; -1.0; -1.0; -1.0; -1.0|]

let ScalableCornerTemplate (glyph: Glyph) (options: SvgOption) scale =
  let mutable set = Set.empty<string>
  let path = ScalableCornerPath glyph options scale
  set <- set.Add (putTick options path.[0] path.[1] path.[2] path.[3])
  set <- set.Add (putTick options path.[4] path.[5] path.[6] path.[7])
  set |> Array.ofSeq |> Array.sort |> Array.fold (fun r s -> r + s) ""

let ScalableRoundCornerPath (glyph: Glyph) (options: SvgOption) scale = 
  let cols = [|4.0; 4.0;  4.0; 0.0; 8.0|] |> Array.map (fun x -> shiftColCoordGridToSvg scale x glyph.gridCoord)
  let rows = [|7.0; 0.0; 14.0; 7.0; 7.0|] |> Array.map (fun x -> shiftRowCoordGridToSvg scale x glyph.gridCoord)
  match glyph.glyphKind with
  | RoundUpperLeftCorner -> [[|cols.[2]; rows.[2]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|]; [||]; [||]; [||]]
  | RoundLowerLeftCorner -> [[|cols.[1]; rows.[1]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|]; [||]; [||]; [||]]
  | RoundUpperRightCorner -> [[|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[2]; rows.[2]|]; [||]; [||]; [||]]
  | RoundLowerRightCorner -> [[|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[1]; rows.[1]|]; [||]; [||]; [||]]
  | RoundUpperLeftAndRightCorner ->
    [[|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[2]; rows.[2]|];
     [|cols.[2]; rows.[2]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|]; [||]; [||]]
  | RoundLowerLeftAndRightCorner ->
    [[|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[1]; rows.[1]|];
     [|cols.[1]; rows.[1]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|]; [||]; [||]]
  | RoundUpperAndLowerRightCorner ->
    [[|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[1]; rows.[1]|];
     [|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[2]; rows.[2]|]; [||]; [||]]
  | RoundUpperAndLowerLeftCorner ->
    [[|cols.[2]; rows.[2]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|];
     [|cols.[1]; rows.[1]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|]; [||]; [||]]
  | RoundCrossCorner ->
    [[|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[2]; rows.[2]|];
     [|cols.[2]; rows.[2]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|];
     [|cols.[3]; rows.[3]; cols.[0]; rows.[0]; cols.[1]; rows.[1]|];
     [|cols.[1]; rows.[1]; cols.[0]; rows.[0]; cols.[4]; rows.[4]|]]
  | _ -> 
#if DEBUG
    Log.logError "Wrong Glyph kind in ScalableRoundCornerPath: %A" glyph 
#endif 
    [[||]; [||]; [||]; [||]]

let ScalableRoundCornerTemplate (glyph: Glyph) (options: SvgOption) scale =
  let arcs = 
    ScalableRoundCornerPath glyph options scale 
    |> List.filter (fun x -> x.Length > 0)
    |> List.map (fun p -> putArc options p.[0] p.[1] p.[2] p.[3] p.[4] p.[5])
  let mutable set = Set.empty<string>
  for arc in arcs do set <- set.Add arc
  set |> Array.ofSeq |> Array.sort |> Array.fold (fun r s -> r + s) ""

let ScalableTextTemplate (text: Text) (options: SvgOption) scale =
  let fontOffsetWidth =
    FontOffsetWidth
    |> getFloatOption options "canvas-font-offset-width"
    |> getFloatOption options "font-offset-width"
  let fontOffsetHeight =
    FontOffsetHeight
    |> getFloatOption options "canvas-font-offset-height"
    |> getFloatOption options "font-offset-height"
  let leadingBlanks = text.text.Length - text.text.TrimStart().Length
  let x = ((float)(text.gridCoord.col + leadingBlanks) * GlyphWidth  + fontOffsetWidth)* scale.colsc
  let y = ((float)(text.gridCoord.row) * GlyphHeight + fontOffsetHeight) * scale.rowsc
  let fontSize = (FontSize * scale.rowsc)
  [|"      <text "
    sprintf "x=\"%.3f\" y=\"%.3f\" " x y
    sprintf "style=\"fill:%s\" " (getStringOption options "fill" Fill)
    sprintf "font-family=\"%s\" " (getStringOption options "font-family" (getStringOption options "canvas-font-family" FontFamily))
    sprintf "font-size=\"%.3f\">\n" (getFloatOption options "font-size" (getFloatOption options "canvas-font-size" fontSize))
    text.text
    "\n      </text>"|]
  |> Array.fold (fun r s -> r + s) ""

let ScalableLineTemplate (line: Line) (options: SvgOption) scale =
  match line.orientation with
  | Horizontal ->
    let col0 = shiftColCoordGridToSvg scale -1.0 line.gridCoordStart
    let row0 = shiftRowCoordGridToSvg scale 7.0 line.gridCoordStart
    let col1 = shiftColCoordGridToSvg scale 9.0 line.gridCoordEnd
    let row1 = shiftRowCoordGridToSvg scale 7.0 line.gridCoordEnd
    putTick options col0 row0 col1 row1
  | Vertical ->
    let col0 = shiftColCoordGridToSvg scale 4.0 line.gridCoordStart
    let row0 = shiftRowCoordGridToSvg scale -1.0 line.gridCoordStart
    let col1 = shiftColCoordGridToSvg scale 4.0 line.gridCoordEnd
    let row1 = shiftRowCoordGridToSvg scale 15.0 line.gridCoordEnd
    putTick options col0 row0 col1 row1
