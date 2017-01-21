using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SingleNoteEnemySkinProvider : EnemySkinProvider {

	[SerializeField]
	private List<KeyNote> notes;

	[SerializeField]
	private List<Sprite> skins;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public override void SetSkin(Chord killerChord)
	{
		Assert.AreEqual(killerChord.notesInChord.Count, 1);
		spriteRenderer.sprite = skins[notes.IndexOf(killerChord.notesInChord[0])];
	}
}
