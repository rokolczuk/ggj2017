using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public static class EventDispatcher {

	public delegate void EventHandler<TEvent>(TEvent e);

	private static Dictionary<Type, List<Delegate>> eventMappings = new Dictionary<Type, List<Delegate>>();

	public static void Dispatch<TEvent>(TEvent e)
	{
		List<Delegate> eventHandlers;
		if (eventMappings.TryGetValue(typeof(TEvent), out eventHandlers)) 
		{
			foreach (var eventHandler in eventHandlers) 
			{
				var specificEventHandler = eventHandler as EventHandler<TEvent>;
				specificEventHandler(e);
			}
		}
	}

	public static void AddEventListener<TEvent>(EventHandler<TEvent> eventHandler)
	{
		List<Delegate> eventHandlers;

		if (eventMappings.TryGetValue(typeof(TEvent), out eventHandlers))
			eventHandlers.Add(eventHandler);
		else
			eventMappings.Add(typeof(TEvent), new List<Delegate> { eventHandler });
	}

	public static void RemoveEventListener<TEvent>(EventHandler<TEvent> eventHandler)
	{
		List<Delegate> eventHandlers;

		if (eventMappings.TryGetValue(typeof(TEvent), out eventHandlers))
		{
			eventHandlers.Remove(eventHandler);
		}
	}
}
