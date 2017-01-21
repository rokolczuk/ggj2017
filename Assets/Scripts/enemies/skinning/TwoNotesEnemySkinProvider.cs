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
	private SpriteRenderer firstNoteLetterSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteLetterSpriteRenderer;

	[SerializeField]
	private SpriteRenderer firstNoteBackgroundSpriteRenderer;

	[SerializeField]
	private SpriteRenderer secondNoteBackgroundSpriteRenderer;


	public override void SetSkin(Chord killerChord)
	{

		Assert.AreEqual(killerChord.notesInChord.Count, 2);

		KeyNote firstKeyNote = killerChord.notesInChord[0];
		KeyNote secondKeyNote = killerChord.notesInChord[1];

		int firstNoteSkinIndex = notes.IndexOf(firstKeyNote);
		int secondNoteSkinIndex = notes.IndexOf(secondKeyNote);

		KeyManager keyManager = GameObject.FindObjectOfType<KeyManager>();

		Color firstNoteColor = keyManager.GetKeyData(firstKeyNote).activeColor;
		Color secondNoteColor = keyManager.GetKeyData(secondKeyNote).activeColor;

		firstNoteLetterSpriteRenderer.sprite = noteLetterSkins[firstNoteSkinIndex];
		secondNoteLetterSpriteRenderer.sprite = noteLetterSkins[secondNoteSkinIndex];

		firstNoteBackgroundSpriteRenderer.color = firstNoteColor;
		secondNoteBackgroundSpriteRenderer.color = secondNoteColor;
	}
}
