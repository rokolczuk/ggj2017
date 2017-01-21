using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TwoNotesEnemySkinProvider : EnemySkinProvider {

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
	private SpriteRenderer firstNoteBackgroundSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteBackgroundSpriteRenderer;

	[SerializeField]
	private SpriteRenderer firstNoteHintSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteHintSpriteRenderer;

	[SerializeField]
	private SpriteRenderer firstNoteTrailSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteTrailSpriteRenderer;


	public override void SetSkin(Chord killerChord)
	{

		Assert.AreEqual(killerChord.notesInChord.Count, 2);

		KeyNote firstKeyNote = killerChord.notesInChord[0];
		KeyNote secondKeyNote = killerChord.notesInChord[1];

		KeyManager keyManager = GameManager.FindObjectOfType<KeyManager>();

		Color firstNoteColor = keyManager.GetKeyData(firstKeyNote).activeColor;
		Color secondNoteColor = keyManager.GetKeyData(secondKeyNote).activeColor;

		int firstNoteSkinIndex = notes.IndexOf(firstKeyNote);
		int secondNoteSkinIndex = notes.IndexOf(secondKeyNote);

		firstNoteLetterSpriteRenderer.sprite = noteLetterSkins[firstNoteSkinIndex];
		secondNoteLetterSpriteRenderer.sprite = noteLetterSkins[secondNoteSkinIndex];

		firstNoteBackgroundSpriteRenderer.material.SetColor("_Color", firstNoteColor);
		secondNoteBackgroundSpriteRenderer.material.SetColor("_Color", secondNoteColor);

		firstNoteHintSpriteRenderer.sprite = noteHintsSkins[firstNoteSkinIndex];
		secondNoteHintSpriteRenderer.sprite = noteHintsSkins[secondNoteSkinIndex];

		firstNoteHintSpriteRenderer.material.SetColor("_Color", firstNoteColor);
		secondNoteHintSpriteRenderer.material.SetColor("_Color", secondNoteColor);

		firstNoteTrailSpriteRenderer.material.SetColor("_Color", firstNoteColor);
		secondNoteTrailSpriteRenderer.material.SetColor("_Color", secondNoteColor);
	}
}
