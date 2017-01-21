
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ThreeNotesEnemySkinProvider : EnemySkinProvider {

	[SerializeField]
	private List<KeyNote> notes;

	[SerializeField]
	private List<Sprite> noteLetterSkins;

	[SerializeField]
	private List<Sprite> noteHintsSkins;

	[SerializeField]
	private SpriteRenderer firstNoteLetterSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteLetterSpriteRenderer;

	[SerializeField]
	private SpriteRenderer thirdNoteLetterSpriteRenderer;


	[SerializeField]
	private SpriteRenderer firstNoteBackgroundSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteBackgroundSpriteRenderer;

	[SerializeField]
	private SpriteRenderer thirdNoteBackgroundSpriteRenderer;

	[SerializeField]
	private SpriteRenderer firstNoteHintSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteHintSpriteRenderer;

	[SerializeField]
	private SpriteRenderer thirdNoteHintSpriteRenderer;

	[SerializeField]
	private SpriteRenderer firstNoteTrailSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteTrailSpriteRenderer;

	[SerializeField]
	private SpriteRenderer thirdNoteTrailSpriteRenderer;


	public override void SetSkin(Chord killerChord)
	{

		Assert.AreEqual(killerChord.notesInChord.Count, 3);

		KeyNote firstKeyNote = killerChord.notesInChord[0];
		KeyNote secondKeyNote = killerChord.notesInChord[1];
		KeyNote thirdKeyNote = killerChord.notesInChord[2];

		KeyManager keyManager = GameManager.FindObjectOfType<KeyManager>();

		Color firstNoteColor = keyManager.GetKeyData(firstKeyNote).activeColor;
		Color secondNoteColor = keyManager.GetKeyData(secondKeyNote).activeColor;
		Color thirdNoteColor = keyManager.GetKeyData(thirdKeyNote).activeColor;

		int firstNoteSkinIndex = notes.IndexOf(firstKeyNote);
		int secondNoteSkinIndex = notes.IndexOf(secondKeyNote);
		int thirdNoteSkinIndex = notes.IndexOf(thirdKeyNote);


		firstNoteLetterSpriteRenderer.sprite = noteLetterSkins[firstNoteSkinIndex];
		secondNoteLetterSpriteRenderer.sprite = noteLetterSkins[secondNoteSkinIndex];
		thirdNoteLetterSpriteRenderer.sprite = noteLetterSkins[secondNoteSkinIndex];

		firstNoteBackgroundSpriteRenderer.material.SetColor("_Color", firstNoteColor);
		secondNoteBackgroundSpriteRenderer.material.SetColor("_Color", secondNoteColor);
		thirdNoteBackgroundSpriteRenderer.material.SetColor("_Color", secondNoteColor);

		firstNoteHintSpriteRenderer.sprite = noteHintsSkins[firstNoteSkinIndex];
		secondNoteHintSpriteRenderer.sprite = noteHintsSkins[secondNoteSkinIndex];
		thirdNoteHintSpriteRenderer.sprite = noteHintsSkins[thirdNoteSkinIndex];

		firstNoteHintSpriteRenderer.material.SetColor("_Color", firstNoteColor);
		secondNoteHintSpriteRenderer.material.SetColor("_Color", secondNoteColor);
		thirdNoteHintSpriteRenderer.material.SetColor("_Color", thirdNoteColor);

		firstNoteTrailSpriteRenderer.material.SetColor("_Color", firstNoteColor);
		secondNoteTrailSpriteRenderer.material.SetColor("_Color", secondNoteColor);
		thirdNoteTrailSpriteRenderer.material.SetColor("_Color", thirdNoteColor);
	}
}
