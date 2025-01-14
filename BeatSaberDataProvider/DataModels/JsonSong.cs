﻿using BeatSaberDataProvider.DataModels;
using BeatSaberDataProvider.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BeatSaberDataProvider
{
    public class JsonSong : IEquatable<JsonSong>
    {
        // Link: https://raw.githubusercontent.com/andruzzzhka/BeatSaberScrappedData/master/combinedScrappedData.json
        private static readonly Regex _digitRegex = new Regex("^[0-9]+$", RegexOptions.Compiled);
        private static readonly Regex _beatSaverRegex = new Regex("^[0-9]+-[0-9]+$", RegexOptions.Compiled);
        public const char IDENTIFIER_DELIMITER = (char)0x220E;
        private const string DOWNLOAD_URL_BASE = "http://beatsaver.com/download/";


        [JsonIgnore]
        private Dictionary<string, float> _rankedDiffs;
        [JsonIgnore]
        public Dictionary<string, float> RankedDifficulties
        {
            get
            {
                if (_rankedDiffs == null)
                    _rankedDiffs = new Dictionary<string, float>();
                if (ScoreSaberInfo.Count != _rankedDiffs.Count) // If they don't have the same number of difficulties, remake
                {
                    _rankedDiffs = new Dictionary<string, float>();
                    foreach (var key in ScoreSaberInfo.Keys)
                    {
                        if (ScoreSaberInfo[key].Ranked)
                        {
                            if (hash == ScoreSaberInfo[key].SongHash)
                                _rankedDiffs.AddOrUpdate(ScoreSaberInfo[key].DifficultyName, ScoreSaberInfo[key].Stars);
                            else
                                Logger.Debug($"Ranked version of {key} is outdated.\n" +
                                    $"   {hash} != {ScoreSaberInfo[key].SongHash}");
                        }
                    }
                }
                return _rankedDiffs;
            }
        }

        public BeatSaverSong BeatSaverInfo { get; set; }

        private Dictionary<int, ScoreSaberDifficulty> _scoreSaberInfo;
        public Dictionary<int, ScoreSaberDifficulty> ScoreSaberInfo
        {
            get
            {
                if (_scoreSaberInfo == null)
                    _scoreSaberInfo = new Dictionary<int, ScoreSaberDifficulty>();
                return _scoreSaberInfo;
            }
            set { _scoreSaberInfo = value; }
        }

        private string _hash;
        /// <summary>
        /// Hash is always uppercase (or empty).
        /// </summary>
        public string hash
        {
            get
            {
                if (string.IsNullOrEmpty(_hash))
                {
                    if (BeatSaverInfo != null)
                        _hash = BeatSaverInfo.hash.ToUpper();
                    else
                    {
                        var ssSong = ScoreSaberInfo.Values.FirstOrDefault();
                        if (ssSong != null)
                            _hash = ssSong.SongHash.ToUpper();
                    }
                }
                return _hash;
            }
        }

        public int keyAsInt
        {
            get
            {
                if (BeatSaverInfo != null)
                    return BeatSaverInfo.KeyAsInt;
                return 0;
            }
        }

        public string key
        {
            get
            {
                if (BeatSaverInfo != null)
                    return BeatSaverInfo.key;
                return string.Empty;
            }
        }

        public string songName
        {
            get
            {
                if (BeatSaverInfo != null)
                    return BeatSaverInfo.metadata.songName;
                var ssSong = ScoreSaberInfo.Values.FirstOrDefault();
                if (ssSong != null)
                    return ssSong.SongName;
                return string.Empty;
            }
        }

        public string authorName
        {
            get
            {
                if (BeatSaverInfo != null)
                    return BeatSaverInfo.uploader.username;
                var ssSong = ScoreSaberInfo.Values.FirstOrDefault();
                if (ssSong != null)
                    return ssSong.LevelAuthorName;
                return string.Empty;
            }
        }

        public double bpm
        {
            get
            {
                if (BeatSaverInfo != null)
                    return BeatSaverInfo.metadata.bpm;
                var ssSong = ScoreSaberInfo.Values.FirstOrDefault();
                if (ssSong != null)
                    return ssSong.BeatsPerMinute;
                return 0;
            }
        }


        /*
        [JsonIgnore]
        private string _identifier;
        [JsonIgnore]
        public string Identifier
        {
            get
            {
                if (string.IsNullOrEmpty(_identifier))
                {
                    if (string.IsNullOrEmpty(hash))
                        return string.Empty;
                    if (string.IsNullOrEmpty(songName))
                        return string.Empty;
                    if (string.IsNullOrEmpty(songSubName))
                        return string.Empty;
                    if (string.IsNullOrEmpty(authorName))
                        return string.Empty;
                    if (bpm <= 0)
                        return string.Empty;
                    _identifier = string.Join(IDENTIFIER_DELIMITER.ToString(), new string[] {
                        hash,
                        songName,
                        songSubName,
                        authorName,
                        bpm.ToString()
                    });
                }
                return _identifier;
            }
        }
        */
        //public SongInfo() { }
        public JsonSong(string hash)
        {
            _hash = hash.ToUpper();
        }

        public override string ToString()
        {
            return hash;
        }

        public object this[string propertyName]
        {
            get
            {
                Type myType = typeof(JsonSong);
                object retVal;
                FieldInfo field = myType.GetField(propertyName);
                if (field != null)
                {
                    retVal = field.GetValue(this);
                }
                else
                {
                    PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                    retVal = myPropInfo.GetValue(this);
                }

                Type whatType = retVal.GetType();
                return retVal;
            }
            set
            {
                Type myType = typeof(JsonSong);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }

        public bool Equals(JsonSong other)
        {
            return hash.ToUpper() == other.hash.ToUpper();
        }
    }



}
