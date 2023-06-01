using System;
using System.Collections.Generic;
using UnityEngine;

public static class NiceColors {
    private static List<Color> _niceColors;

    public static readonly Color Green = GetColor("#6d8d7a");
    public static readonly Color Red = GetColor("#e44132");
    public static readonly Color Blue = GetColor("#0099cc");

    public static List<Color> Colors {
        get {
            if (_niceColors == null) {
                _niceColors = new List<Color>();
                // from http://www.color-hex.com/
                _niceColors.AddRange(new[] {
                    GetColor("#dddcd2"), GetColor("#4f4f65"), GetColor("#787bd3"), GetColor("#310e0f"), GetColor("#f7484e"), GetColor("#747752"),
                    GetColor("#e8eea4"),
                    GetColor("#233032"), GetColor("#b1f4fa"), GetColor("#5c4026"), GetColor("#b96c26"), GetColor("#344613"), GetColor("#ead340"),
                    GetColor("#1a2b34"),
                    GetColor("#355668"), GetColor("#7a3a57"), GetColor("#df1b73"), GetColor("#47353f"), GetColor("#edb1d3"), GetColor("#363632"),
                    GetColor("#b6b7a7"),
                    GetColor("#412201"), GetColor("#d97205"), GetColor("#3b3044"), GetColor("#776188"), GetColor("#0f192b"), GetColor("#4d7ed8"),
                    GetColor("#353315")
                });
            }

            return _niceColors;
        }
    }

    public static Color GetColor(string hexa) {
        if (ColorUtility.TryParseHtmlString(hexa, out var result)) {
            return result;
        }

        throw new Exception("Invalid hexa color " + hexa);
    }
}