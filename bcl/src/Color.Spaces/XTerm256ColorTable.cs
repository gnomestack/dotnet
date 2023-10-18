namespace GnomeStack.ColorSpaces;

internal static class XTerm256ColorTable
{
    public static ReadOnlySpan<uint> Values => new uint[]
    {
        0x000000, // Black
        0x800000, // Maroon
        0x008000, // Green
        0x808000, // Olive
        0x000080, // Navy
        0x800080, // Purple
        0x008080, // Teal
        0xc0c0c0, // Silver
        0x808080, // Grey
        0xff0000, // Red
        0x00ff00, // Lime
        0xffff00, // Yellow
        0x0000ff, // Blue
        0xff00ff, // Fuchsia
        0x00ffff, // Aqua
        0xffffff, // White
        0x000000, // Grey0
        0x00005f, // NavyBlue
        0x000087, // DarkBlue
        0x0000af, // Blue3
        0x0000d7, // Blue3
        0x0000ff, // Blue1
        0x005f00, // DarkGreen
        0x005f5f, // DeepSkyBlue4
        0x005f87, // DeepSkyBlue4
        0x005faf, // DeepSkyBlue4
        0x005fd7, // DodgerBlue3
        0x005fff, // DodgerBlue2
        0x008700, // Green4
        0x00875f, // SpringGreen4
        0x008787, // Turquoise4
        0x0087af, // DeepSkyBlue3
        0x0087d7, // DeepSkyBlue3
        0x0087ff, // DodgerBlue1
        0x00af00, // Green3
        0x00af5f, // SpringGreen3
        0x00af87, // DarkCyan
        0x00afaf, // LightSeaGreen
        0x00afd7, // DeepSkyBlue2
        0x00afff, // DeepSkyBlue1
        0x00d700, // Green3
        0x00d75f, // SpringGreen3
        0x00d787, // SpringGreen2
        0x00d7af, // Cyan3
        0x00d7d7, // DarkTurquoise
        0x00d7ff, // Turquoise2
        0x00ff00, // Green1
        0x00ff5f, // SpringGreen2
        0x00ff87, // SpringGreen1
        0x00ffaf, // MediumSpringGreen
        0x00ffd7, // Cyan2
        0x00ffff, // Cyan1
        0x5f0000, // DarkRed
        0x5f005f, // DeepPink4
        0x5f0087, // Purple4
        0x5f00af, // Purple4
        0x5f00d7, // Purple3
        0x5f00ff, // BlueViolet
        0x5f5f00, // Orange4
        0x5f5f5f, // Grey37
        0x5f5f87, // MediumPurple4
        0x5f5faf, // SlateBlue3
        0x5f5fd7, // SlateBlue3
        0x5f5fff, // RoyalBlue1
        0x5f8700, // Chartreuse4
        0x5f875f, // DarkSeaGreen4
        0x5f8787, // PaleTurquoise4
        0x5f87af, // SteelBlue
        0x5f87d7, // SteelBlue3
        0x5f87ff, // CornflowerBlue
        0x5faf00, // Chartreuse3
        0x5faf5f, // DarkSeaGreen4
        0x5faf87, // CadetBlue
        0x5fafaf, // CadetBlue
        0x5fafd7, // SkyBlue3
        0x5fafff, // SteelBlue1
        0x5fd700, // Chartreuse3
        0x5fd75f, // PaleGreen3
        0x5fd787, // SeaGreen3
        0x5fd7af, // Aquamarine3
        0x5fd7d7, // MediumTurquoise
        0x5fd7ff, // SteelBlue1
        0x5fff00, // Chartreuse2
        0x5fff5f, // SeaGreen2
        0x5fff87, // SeaGreen1
        0x5fffaf, // SeaGreen1
        0x5fffd7, // Aquamarine1
        0x5fffff, // DarkSlateGray2
        0x870000, // DarkRed
        0x87005f, // DeepPink4
        0x870087, // DarkMagenta
        0x8700af, // DarkMagenta
        0x8700d7, // DarkViolet
        0x8700ff, // Purple
        0x875f00, // Orange4
        0x875f5f, // LightPink4
        0x875f87, // Plum4
        0x875faf, // MediumPurple3
        0x875fd7, // MediumPurple3
        0x875fff, // SlateBlue1
        0x878700, // Yellow4
        0x87875f, // Wheat4
        0x878787, // Grey53
        0x8787af, // LightSlateGrey
        0x8787d7, // MediumPurple
        0x8787ff, // LightSlateBlue
        0x87af00, // Yellow4
        0x87af5f, // DarkOliveGreen3
        0x87af87, // DarkSeaGreen
        0x87afaf, // LightSkyBlue3
        0x87afd7, // LightSkyBlue3
        0x87afff, // SkyBlue2
        0x87d700, // Chartreuse2
        0x87d75f, // DarkOliveGreen3
        0x87d787, // PaleGreen3
        0x87d7af, // DarkSeaGreen3
        0x87d7d7, // DarkSlateGray3
        0x87d7ff, // SkyBlue1
        0x87ff00, // Chartreuse1
        0x87ff5f, // LightGreen
        0x87ff87, // LightGreen
        0x87ffaf, // PaleGreen1
        0x87ffd7, // Aquamarine1
        0x87ffff, // DarkSlateGray1
        0xaf0000, // Red3
        0xaf005f, // DeepPink4
        0xaf0087, // MediumVioletRed
        0xaf00af, // Magenta3
        0xaf00d7, // DarkViolet  // do the rest
        0xaf00ff, // Purple
        0xaf5f00, // DarkOrange3
        0xaf5f5f, // IndianRed
        0xaf5f87, // HotPink3
        0xaf5faf, // MediumOrchid3
        0xaf5fd7, // MediumOrchid
        0xaf5fff, // MediumPurple2
        0xaf8700, // DarkGoldenrod
        0xaf875f, // LightSalmon3
        0xaf8787, // RosyBrown
        0xaf87af, // Grey63
        0xaf87d7, // MediumPurple2
        0xaf87ff, // MediumPurple1
        0xafaf00, // Gold3
        0xafaf5f, // DarkKhaki
        0xafaf87, // NavajoWhite3
        0xafafaf, // Grey69
        0xafafd7, // LightSteelBlue3
        0xafafff, // LightSteelBlue
        0xafd700, // Yellow3
        0xafd75f, // DarkOliveGreen3
        0xafd787, // DarkSeaGreen3
        0xafd7af, // DarkSeaGreen2
        0xafd7d7, // LightCyan3
        0xafd7ff, // LightSkyBlue1
        0xafff00, // GreenYellow
        0xafff5f, // DarkOliveGreen2
        0xafff87, // PaleGreen1
        0xafffaf, // DarkSeaGreen2
        0xafffd7, // DarkSeaGreen1
        0xafffff, // PaleTurquoise1
        0xd70000, // Red3
        0xd7005f, // DeepPink3
        0xd70087, // DeepPink3
        0xd700af, // Magenta3
        0xd700d7, // Magenta3
        0xd700ff, // Magenta2
        0xd75f00, // DarkOrange3
        0xd75f5f, // IndianRed
        0xd75f87, // HotPink3
        0xd75faf, // HotPink2
        0xd75fd7, // Orchid
        0xd75fff, // MediumOrchid1
        0xd78700, // Orange3
        0xd7875f, // LightSalmon3
        0xd78787, // LightPink3
        0xd787af, // Pink3
        0xd787d7, // Plum3
        0xd787ff, // Violet
        0xd7af00, // Gold3
        0xd7af5f, // LightGoldenrod3
        0xd7af87, // Tan
        0xd7afaf, // MistyRose3
        0xd7afd7, // Thistle3
        0xd7afff, // Plum2
        0xd7d700, // Yellow3
        0xd7d75f, // Khaki3
        0xd7d787, // LightGoldenrod2
        0xd7d7af, // LightYellow3
        0xd7d7d7, // Grey84
        0xd7d7ff, // LightSteelBlue1
        0xd7ff00, // Yellow2
        0xd7ff5f, // DarkOliveGreen1
        0xd7ff87, // DarkOliveGreen1
        0xd7ffaf, // DarkSeaGreen1
        0xd7ffd7, // Honeydew2
        0xd7ffff, // LightCyan1
        0xff0000, // Red1
        0xff005f, // DeepPink2
        0xff0087, // DeepPink1
        0xff00af, // DeepPink1
        0xff00d7, // Magenta2
        0xff00ff, // Magenta1
        0xff5f00, // OrangeRed1
        0xff5f5f, // IndianRed1
        0xff5f87, // IndianRed1
        0xff5faf, // HotPink
        0xff5fd7, // HotPink
        0xff5fff, // MediumOrchid1
        0xff8700, // DarkOrange
        0xff875f, // Salmon1
        0xff8787, // LightCoral
        0xff87af, // PaleVioletRed1
        0xff87d7, // Orchid2
        0xff87ff, // Orchid1
        0xffaf00, // Orange1
        0xffaf5f, // SandyBrown
        0xffaf87, // LightSalmon1
        0xffafaf, // LightPink1
        0xffafd7, // Pink1
        0xffafff, // Plum1
        0xffd700, // Gold1
        0xffd75f, // LightGoldenrod2
        0xffd787, // LightGoldenrod2
        0xffd7af, // NavajoWhite1
        0xffd7d7, // MistyRose1
        0xffd7ff, // Thistle1
        0xffff00, // Yellow1
        0xffff5f, // LightGoldenrod1
        0xffff87, // Khaki1
        0xffffaf, // Wheat1
        0xffffd7, // Cornsilk1
        0xffffff, // Grey100
        0x080808, // Grey3
        0x121212, // Grey7
        0x1c1c1c, // Grey11
        0x262626, // Grey15
        0x303030, // Grey19
        0x3a3a3a, // Grey23
        0x444444, // Grey27
        0x4e4e4e, // Grey30
        0x585858, // Grey35
        0x626262, // Grey39
        0x6c6c6c, // Grey42
        0x767676, // Grey46
        0x808080, // Grey50
        0x8a8a8a, // Grey54
        0x949494, // Grey58
        0x9e9e9e, // Grey62
        0xa8a8a8, // Grey66
        0xb2b2b2, // Grey70
        0xbcbcbc, // Grey74
        0xc6c6c6, // Grey78
        0xd0d0d0, // Grey82
        0xdadada, // Grey85
        0xe4e4e4, // Grey89
        0xeeeeee, // Grey93
    };
/*
    AliceBlue = 0xf0f8ff,
    AntiqueWhite = 0xfaebd7,
    Aqua = 0x00ffff,

    AquaMarine = 0x7fffd4,
    azure: '#f0ffff',
    beige: '#f5f5dc',
    bisque: '#ffe4c4',
    black: '#000000',
    blanchedalmond: '#ffebcd',
    blue: '#0000ff',
    blueviolet: '#8a2be2',
    brown: '#a52a2a',
    burlywood: '#deb887',
    cadetblue: '#5f9ea0',
    chartreuse: '#7fff00',
    chocolate: '#d2691e',
    coral: '#ff7f50',
    cornflower: '#6495ed',
    cornflowerblue: '#6495ed',
    cornsilk: '#fff8dc',
    crimson: '#dc143c',
    cyan: '#00ffff',
    darkblue: '#00008b',
    darkcyan: '#008b8b',
    darkgoldenrod: '#b8860b',
    darkgray: '#a9a9a9',
    darkgreen: '#006400',
    darkgrey: '#a9a9a9',
    darkkhaki: '#bdb76b',
    darkmagenta: '#8b008b',
    darkolivegreen: '#556b2f',
    darkorange: '#ff8c00',
    darkorchid: '#9932cc',
    darkred: '#8b0000',
    darksalmon: '#e9967a',
    darkseagreen: '#8fbc8f',
    darkslateblue: '#483d8b',
    darkslategray: '#2f4f4f',
    darkslategrey: '#2f4f4f',
    darkturquoise: '#00ced1',
    darkviolet: '#9400d3',
    deeppink: '#ff1493',
    deepskyblue: '#00bfff',
    dimgray: '#696969',
    dimgrey: '#696969',
    dodgerblue: '#1e90ff',
    firebrick: '#b22222',
    floralwhite: '#fffaf0',
    forestgreen: '#228b22',
    fuchsia: '#ff00ff',
    gainsboro: '#dcdcdc',
    ghostwhite: '#f8f8ff',
    gold: '#ffd700',
    goldenrod: '#daa520',
    gray: '#808080',
    green: '#008000',
    greenyellow: '#adff2f',
    grey: '#808080',
    honeydew: '#f0fff0',
    hotpink: '#ff69b4',
    indianred: '#cd5c5c',
    indigo: '#4b0082',
    ivory: '#fffff0',
    khaki: '#f0e68c',
    laserlemon: '#ffff54',
    lavender: '#e6e6fa',
    lavenderblush: '#fff0f5',
    lawngreen: '#7cfc00',
    lemonchiffon: '#fffacd',
    lightblue: '#add8e6',
    lightcoral: '#f08080',
    lightcyan: '#e0ffff',
    lightgoldenrod: '#fafad2',
    lightgoldenrodyellow: '#fafad2',
    lightgray: '#d3d3d3',
    lightgreen: '#90ee90',
    lightgrey: '#d3d3d3',
    lightpink: '#ffb6c1',
    lightsalmon: '#ffa07a',
    lightseagreen: '#20b2aa',
    lightskyblue: '#87cefa',
    lightslategray: '#778899',
    lightslategrey: '#778899',
    lightsteelblue: '#b0c4de',
    lightyellow: '#ffffe0',
    lime: '#00ff00',
    limegreen: '#32cd32',
    linen: '#faf0e6',
    magenta: '#ff00ff',
    maroon: '#800000',
    maroon2: '#7f0000',
    maroon3: '#b03060',
    mediumaquamarine: '#66cdaa',
    mediumblue: '#0000cd',
    mediumorchid: '#ba55d3',
    mediumpurple: '#9370db',
    mediumseagreen: '#3cb371',
    mediumslateblue: '#7b68ee',
    mediumspringgreen: '#00fa9a',
    mediumturquoise: '#48d1cc',
    mediumvioletred: '#c71585',
    midnightblue: '#191970',
    mintcream: '#f5fffa',
    mistyrose: '#ffe4e1',
    moccasin: '#ffe4b5',
    navajowhite: '#ffdead',
    navy: '#000080',
    oldlace: '#fdf5e6',
    olive: '#808000',
    olivedrab: '#6b8e23',
    orange: '#ffa500',
    orangered: '#ff4500',
    orchid: '#da70d6',
    palegoldenrod: '#eee8aa',
    palegreen: '#98fb98',
    paleturquoise: '#afeeee',
    palevioletred: '#db7093',
    papayawhip: '#ffefd5',
    peachpuff: '#ffdab9',
    peru: '#cd853f',
    pink: '#ffc0cb',
    plum: '#dda0dd',
    powderblue: '#b0e0e6',
    purple: '#800080',
    purple2: '#7f007f',
    purple3: '#a020f0',
    rebeccapurple: '#663399',
    red: '#ff0000',
    rosybrown: '#bc8f8f',
    royalblue: '#4169e1',
    saddlebrown: '#8b4513',
    salmon: '#fa8072',
    sandybrown: '#f4a460',
    seagreen: '#2e8b57',
    seashell: '#fff5ee',
    sienna: '#a0522d',
    silver: '#c0c0c0',
    skyblue: '#87ceeb',
    slateblue: '#6a5acd',
    slategray: '#708090',
    slategrey: '#708090',
    snow: '#fffafa',
    springgreen: '#00ff7f',
    steelblue: '#4682b4',
    tan: '#d2b48c',
    teal: '#008080',
    thistle: '#d8bfd8',
    tomato: '#ff6347',
    turquoise: '#40e0d0',
    violet: '#ee82ee',
    wheat: '#f5deb3',
    white: '#ffffff',
    whitesmoke: '#f5f5f5',
    yellow: '#ffff00',
    yellowgreen: '#9acd32'
    */
}