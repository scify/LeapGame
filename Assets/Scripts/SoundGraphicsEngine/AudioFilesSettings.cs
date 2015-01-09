/**
 * This file contains the AudioFilesSettings class. 
 * 
 * This class is responsible for loading and handling
 * the audio settings. Audio settings contain the amount
 * of players that the game can hold, the cases at which
 * sound will be reproduced, the corresponding positions of
 * sound reproduction and the relative (to the application)
 * paths of the sounds. 
 * 
 * //TODO Add legal stuff.... (have to? )
 * 
 * @file AudioFilesSettings.cs
 * @author Konstantinos Drossos
 * @copyright ??? distributed as is under MIT Licence.
 */ 

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

/**
 * The audio file settings for players.
 * 
 * This class handles the audio file settings for
 * all players in a specified game. 
 * 
 * @access Public
 * @author Konstantinos Drossos
 */
public class AudioFilesSettings {

	/** 
	 * Constructor of the AudioFilesSettings class.
	 * 
	 * The constructor of the AudioFilesSettings class accepts
	 * the game name and sets up the audio files' settings for the
	 * specified game. 
	 * 
	 * @param gameName - the name of the game to be played (string)
	 * @access Public
	 * @author Konstantinos Drossos
	 */
	public AudioFilesSettings (string gameName) {

		/* Initiliase the Lists that are used  if havent' been */
		if (this.audioFilesPerPlayer == null) this.audioFilesPerPlayer = new List<List<AudioFileForGame>> ();

		try {
			/* Get the Xml Document */
			XmlDocument gameSettings = this.xmlParsingGetTheSettingsDocument(gameName);

			/* Parse the general section */
			this.xmlParsingGeneralSection(ref gameSettings);

			/* Parse the audio files section */
			this.xmlParsingAudioFileSection(ref gameSettings);

		} catch (Exception e) {
            Debug.Log(e.Message); throw e;
		}
	} /* End public AudioFilesSettings */



	/**
	 * Gets the path of a sound for a given player and position.
	 * 
	 * This public method accepts the player's index, the case and the sound origin
	 * and returns the path of the appropriate sound.
	 * 
	 * @param player - the index of the player (int)
	 * @param theCase - the case of the sound (string)
	 * @param soundOrigin - the origin of the sound (UnityEngine.Vector3)
	 * @return string - the path of the sound
	 * @access Public
	 * @author Konstantinos Drossos
	 */
	public string getSoundForPlayer (int player, string theCase, UnityEngine.Vector3 soundOrigin) {

		return this.audioFilesPerPlayer [player].Find (
			delegate (AudioFileForGame af) {
				if (af.TheCase.Equals (theCase) && af.ThePosition.Equals (soundOrigin)) return true;
				else return false;
			}).ThePath;
	} /* End public string getSoundForPlayer (int player, string theCase, UnityEngine.Vector3 soundOrigin) */



	/**
	 * Returns the cases for sound reproduction.
	 * 
	 * This public method returns all acceptable cases
	 * for sound reproduction of the particular game.
	 * 
	 * @return List<string> - the cases
	 * @access Public
	 * @author Konstantinos Drossos
	 */
	public List<string> getAllSoundsCases() {
		return null;
		//return this.audioFilesCases;
	} /* End public List<string> getAllSoundsCases() */



	/**
	 * Returns the position of sound for a given case and player.
	 * 
	 * This public method accepts a verbal description of position
	 * and returns the actual vector with position for that case.
	 *
	 * @param nOfPlayer - the index of player (int)
	 * @param theCase - the verbal description of position (string)
	 * @return UnityEngine.Vector3 - the position
	 * @exception ArgumentOutOfRangeException - if the index of player is out of range
	 * @exception KeyNotFoundException - if the verbal description does not exists
	 * @access Public
	 * @author Konstantinos Drossos
	 */
	public UnityEngine.Vector3 getSoundPosition(int nOfPlayer, string theCase) {

		/* Check for bad arguments */
		if (!this.existsPlayer (nOfPlayer) ) 
			throw new ArgumentOutOfRangeException("nOfPlayer", nOfPlayer, AudioFilesSettings.messageGreaterPlayerIndex);
		if (!this.existsCase (theCase)) 
			throw new KeyNotFoundException ("Case: " + theCase + " not found!");

		/* Find the case based on player index */


		return new UnityEngine.Vector3();
	} /* End public UnityEngine.Vector3 getSoundPosition(int nOfPlayer, string theCase) */



	/**
	 * Returns all paths of sounds for a player.
	 * 
	 * This public method accepts a player index and 
	 * returns the paths of all sounds for that player.
	 * 
	 * @param nOfPlayer - the index of player (int)
	 * @return List<string> - the paths of all sounds for the player
	 * @exception ArgumentOutOfRangeException - if the index of player is out of range
	 * @access Public
	 * @author Konstantinos Drossos
	 */
	public List<string> getPathForAllSoundsOfPlayer (int nOfPlayer) {

		if (this.existsPlayer(nOfPlayer))
			throw new ArgumentOutOfRangeException ("nOfPlayer", 
			                                       nOfPlayer, AudioFilesSettings.messageGreaterPlayerIndex);

		//return this.audioFilesPath [nOfPlayer];
		return null;
	} /* End public List<string> getPathForAllSoundsOfPlayer (int nOfPlayer) */



	/**
	 * Gets the specific audio files paths for all players.
	 * 
	 * This public method returns the actual paths in which
	 * are the audio files for all players. The paths are 
	 * returned in a List<string> object where the index of the
	 * strings is the corresponding index of the player. 
	 * 
	 * @return List<string> - the actual audio files' paths for all players
	 * @access Public
	 * @author Konstantinos Drossos
	 */ 
	public List<string> getPlayersPaths() {
		return null;
	} /* End public List<string> getPlayersPaths() */



	public string getPlayerSettingsName (int nOfPlayer) {
		return null;
	}



	public void setPlayerPath(int nOfPlayer, string newPath) {
	}

	public string getPathForSound(int nOfPlayer, string theCase, Vector3 position) {

		return null;
	}



	/**
	 * Gets the xml document containing the audio settings.
	 * 
	 * This private method gets the xml document which contains the
	 * audio settings. It cares of the existence of document and the
	 * sanity of path. 
	 * 
	 * @param gameName - the game for which the settings will be parsed (string)
	 * @return XmlDocument - the XML document with the settings
	 * @access Private
	 * @author Konstantinos Drossos
	 */
	private XmlDocument xmlParsingGetTheSettingsDocument(string gameName) {

		/* Check just in case */
		if (!Directory.Exists(AudioFilesSettings.settingsBaseDir))
			throw new ApplicationException ("Error to audio settings path!" + 
			                                AudioFilesSettings.messageParentPathNotFound);

		/* Get the base directory of the audio settings */
		DirectoryInfo theInfo = new DirectoryInfo(AudioFilesSettings.settingsBaseDir);
		
		FileInfo[] files = theInfo.GetFiles("*_" + gameName + ".xml");
		
		if (files.Length < 1) throw new FileNotFoundException ("Audio settings file not found!");

		XmlDocument gameSettings = new XmlDocument();
		gameSettings.Load(AudioFilesSettings.settingsBaseDir + files[0].Name);

		return gameSettings;
	}



	/**
	 * Parses the general section of the settings' document.
	 * 
	 * This private method parses the general section of the 
	 * settings' xml document. It accepts a reference of the XML
	 * document containing the settings. 
	 * 
	 * @param xmlDoc - the XML document with the settings (ref XmlDocument)
	 * @access Private
	 * @author Konstantinos Drossos
	 */
	private void xmlParsingGeneralSection(ref XmlDocument xmlDoc) {
		this.xmlParsingGetPlayersAmount (ref xmlDoc);
	}



	/**
	 * 
	 */
	private void xmlParsingAudioFileSection(ref XmlDocument xmlDoc) {
		XmlNode audioNode = xmlDoc.DocumentElement.SelectSingleNode("/theFile/audio");
		int playerTmpIndx;
		string tmpString;
		string[] posStrings;
		string tmpPath;
		
		foreach (XmlNode playerNode in audioNode.ChildNodes) {
			
			/* The player index */
			playerTmpIndx = Convert.ToInt16(playerNode.Attributes["index"].InnerText) - 1;
			this.audioFilesPerPlayer.Add(new List<AudioFileForGame>());
			
			/* Iterate through XML nodes and get the info needed */
			foreach(XmlNode tmpSetting in playerNode.ChildNodes) {
				
				/* Iterate through the actual settings nodes */
				foreach(XmlNode tmpAudioFile in tmpSetting.ChildNodes) {
					
					/* Get the position values */
					tmpString = tmpAudioFile.SelectSingleNode("position_vals").InnerText;
					posStrings = tmpString.Split('=');
					tmpPath = tmpAudioFile.SelectSingleNode("path").InnerText;
					
					/* Treat possible quotation marks */
					if (tmpPath.Substring(0, 1).Equals("\"")) 
						tmpPath = tmpPath.Substring(1);
					if (tmpPath.Substring(tmpPath.Length-1, 1).Equals("\"")) 
						tmpPath = tmpPath.Substring(0, tmpPath.Length-1);
					
					/* Check for file ending and if does, remove it */
					if (!tmpPath.Substring(tmpPath.Length - 4).Equals("."))
						tmpPath = tmpPath.Substring(0, tmpPath.Length-4);
					
					
					this.audioFilesPerPlayer[playerTmpIndx].Add(
						new AudioFileForGame(tmpAudioFile.Attributes["case"].InnerText,
	                    new UnityEngine.Vector3(Convert.ToSingle(posStrings[0]), 
		                        Convert.ToSingle(posStrings[1]), 
		                        Convert.ToSingle(posStrings[2])),
	                    "Sounds/" + tmpPath));
				}
			}
		}
	} /* End private void xmlParsingAudioFileSection(ref XmlDocument xmlDoc) */



	/**
	 * Gets the players' amount from settings.
	 * 
	 * This private method parses the xml document which contains the
	 * audio settings and extracts the maximum and minimum amount of
	 * players. The amount of players is assigned to the appropriate 
	 * members of the AudioFilesSettings object. The xml document 
	 * must be passed as reference. 
	 * 
	 * @param xmlDoc - the XML document (ref XmlDocument)
	 * @access Private
	 * @author Konstantinos Drossos
	 */
	private void xmlParsingGetPlayersAmount(ref XmlDocument xmlDoc) {
		/* Get the node for maximum players */
		XmlNode playersNode = xmlDoc.DocumentElement.SelectSingleNode (
			AudioFilesSettings.xmlSettingsNameOfNodeBase + 
			AudioFilesSettings.xmlSettingsNameOfNodeGeneralSettings + 
			AudioFilesSettings.xmlSettingsNameOfNodePlayers + 
			AudioFilesSettings.xmlSettingsNameOfNodeMaxPlayers);

		/* Assign their number */
		this.nOfPlayers_max = Convert.ToInt16(playersNode.InnerText);
		
		/* Get the node for minimum players */
		playersNode = xmlDoc.DocumentElement.SelectSingleNode (
			AudioFilesSettings.xmlSettingsNameOfNodeBase + 
			AudioFilesSettings.xmlSettingsNameOfNodeGeneralSettings + 
			AudioFilesSettings.xmlSettingsNameOfNodePlayers + 
			AudioFilesSettings.xmlSettingsNameOfNodeMinPlayers);

		/* Assign their number */
		this.nOfPlayers_min = Convert.ToInt16(playersNode.InnerText);
	} /* End private void xmlParsingGetPlayersAmount(ref XmlDocument xmlDoc) */


	/**
	 * Check if a player exists.
	 * 
	 * This private method checks if a player exists by
	 * checking if the specified player's index is in range.
	 * 
	 * @param nOfPlayer - the index of the player (int)
	 * @return bool - True if the player exists, false otherwise
	 * @access Private
	 * @author Konstantinos Drossos
	 */ 
	private bool existsPlayer (int nOfPlayer) {
		if (nOfPlayer > this.nOfPlayers_max || nOfPlayer < this.nOfPlayers_min) return false;
		else return true;
	} /* End private bool playerExists (int nOfPlayer) */



	/**
	 * Check if a case exists.
	 * 
	 * This private method checks if a case exists. If it does
	 * not returns false. Otherwise returns true. 
	 * 
	 * @param theCase - the case to check (string)
	 * @return bool - true if exists, false otherwise
	 * @access Private
	 * @author Konstantinos Drossos
	 */
	private bool existsCase (string theCase) {
		//return this.audioFilesCases.Exists (c => c.Equals(theCase));
		return true;
	} /* End private bool existsCase (string theCase) */

	/* ==============  Members section ==============  */

	/*!< Private class for gathering the audio files */
	private class AudioFileForGame {

		public AudioFileForGame(string theCase, UnityEngine.Vector3 thePosition, string thePath) {
			this.theCase = theCase;
			this.thePosition = thePosition;
			this.thePath = thePath;
		}

		public string TheCase {
			get { return theCase; }
			set { theCase = value; }
		}

		public UnityEngine.Vector3 ThePosition {
			get { return thePosition; }
			set { thePosition = value; }
		}

		public string ThePath {
			get { return thePath; }
			set { thePath = value; }
		}

		private string theCase; /*!< The case of sound reproduction */
		private UnityEngine.Vector3 thePosition; /*!< The position of sound reproduction */
		private string thePath; /*!< The path of sound file */
	}
	
	private int nOfPlayers_max;	/*!< Number of maximum players for the game */
	private int nOfPlayers_min;	/*!< Number of minimum players for the game */

	private List<List<AudioFileForGame> > audioFilesPerPlayer; /*!< The audio files for each player */
	private List<string> audioCases; /*!< The cases for audio reproduction */
	private List<string> audioSettings; /*!< The available audio settings */

    private static string settingsBaseDir = Application.dataPath + "/Resources/Sounds/"; /*!< Default base dir for the settings file */
	private static string messageGreaterPlayerIndex = "The specified index of player is greater than the amount of total players"; /*!< Message for exception of player index */
	private static string messageParentPathNotFound = "Path does not exist"; /*!< Message for path not exists */

	/*!< The base node of the settings' xml file */
	private static string xmlSettingsNameOfNodeBase = "/theFile"; 
	/*!< The general settings node of xml file */
	private static string xmlSettingsNameOfNodeGeneralSettings = "/general"; 
	/*!< The player's node of xml file */
	private static string xmlSettingsNameOfNodePlayers = "/players";
	/*!< The max players' node of xml file */
	private static string xmlSettingsNameOfNodeMaxPlayers = "/maximum";
	/*!< The min players' node of xml file */
	private static string xmlSettingsNameOfNodeMinPlayers = "/minimum";
}

/* Scripts/SoundGraphicsEngine/AudioFilesSettings.cs */
/* END OF FILE */
