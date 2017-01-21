using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TwoNotesEnemySkinProvider : EnemySkinProvider {

	[SerializeField]
	private List<KeyNote> notes;

	[SerializeField]
	private List<Sprite> firstNoteSkins;

	[SerializeField]
	private List<Sprite> secondNoteSkins;

	[SerializeField]
	private SpriteRenderer firstNoteSpriteRenderer;

	[SerializeField]
	private SpriteRenderer firstNoteSpriteRenderer;

	public override void SetSkin(Chord killerChord)
	{
		Assert.AreEqual(killerChord.notesInChord.Count, 2);

	
	}
}
