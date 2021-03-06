﻿/**
 * Copyright (C) 2005-2014 by Rivello Multimedia Consulting (RMC).                    
 * code [at] RivelloMultimediaConsulting [dot] com                                                  
 *                                                                      
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the      
 * "Software"), to deal in the Software without restriction, including  
 * without limitation the rights to use, copy, modify, merge, publish,  
 * distribute, sublicense, and#or sell copies of the Software, and to   
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:                                            
 *                                                                      
 * The above copyright notice and this permission notice shall be       
 * included in all copies or substantial portions of the Software.      
 *                                                                      
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,      
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF   
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR    
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.                                      
 */
// Marks the right margin of code *******************************************************************


//--------------------------------------
//  Imports
//--------------------------------------
using UnityEngine;
using strange.extensions.mediation.impl;
using com.rmc.projects.spider_strike.mvcs.controller.signals;
using System.Collections;
using com.rmc.projects.spider_strike.mvcs.model.vo;

//--------------------------------------
//  Namespace
//--------------------------------------
namespace com.rmc.projects.spider_strike.mvcs.view.ui
{
	
	//--------------------------------------
	//  Namespace Properties
	//--------------------------------------
	public enum EnemyPlacement
	{
		DEBUG,
		PRODUCTION

	}
	
	
	//--------------------------------------
	//  Class Attributes
	//--------------------------------------
	
	//--------------------------------------
	//  Class
	//--------------------------------------
	public class GameManagerUI : View 
	{
		
		//--------------------------------------
		//  Properties
		//--------------------------------------
		
		// GETTER / SETTER
		
		// PUBLIC
		/// <summary>
		/// When the enemy parent game object.
		/// </summary>
		public GameObject enemyParentGameObject;

		/// <summary>
		/// When the spider game object.
		/// </summary>
		public GameObject spiderPrefabGameObject;

		/// <summary>
		/// When the intro user interface game object.
		/// </summary>
		public GameObject introUIGameObject;

		/// <summary>
		/// When the target game object.
		/// </summary>
		public TurretUI targetGameObject;

		/// <summary>
		/// When the attack sphere game object. 
		/// NOTE: Non-visual. Sets radius for spawning
		/// </summary>
		public GameObject spawnSphereGameObject;
		
		/// <summary>
		/// When the attack sphere game object. 
		/// NOTE: Non-visual. Sets radius for attack behavior
		/// </summary>
		public GameObject attackSphereGameObject;


		/// <summary>
		/// When the enemy placement. TOGGLED ONLY DURING DEBUGGING
		/// </summary>
		public EnemyPlacement enemyPlacement = EnemyPlacement.PRODUCTION;

		/// <summary>
		/// Gets or sets all views initialized signal.
		/// </summary>
		/// <value>All views initialized signal.</value>
		[Inject]
		public AllViewsInitializedSignal allViewsInitializedSignal {get; set;}



		// PUBLIC STATIC
		
		// PRIVATE
		
		// PRIVATE STATIC
		
		//--------------------------------------
		//  Methods
		//--------------------------------------		
		///<summary>
		///	Use this for initialization
		///</summary>
		override protected void Start () 
		{
			
			base.Start();
			StartCoroutine ( Wait_ThenAllViewsInitializedSignal ());

			//WE DISABLE THE INTRO TO EASE THE USE OF THE UNITY IDE
			//HERE WE ENABLE IT
			introUIGameObject.SetActive (true);


		}


		/// <summary>
		/// Wait_s the then all views initialized signal.
		/// </summary>
		/// <returns>The then all views initialized signal.</returns>
		public IEnumerator Wait_ThenAllViewsInitializedSignal ()
		{

			/*
			 * TODO: WHAT IS THE BEST WAY TO NOTIFY THE MVCS SYSTEM THAT THE VIEWS ARE READY?
			 * 
			 * THIS IS A FULLY FUNCTIONAL WORKAROUND, BUT WE ARE MANUALLY 'WAITING'
			 * 
			 * 
			 **/
			yield return new  WaitForEndOfFrame ();


			//
			allViewsInitializedSignal.Dispatch ();

		}
		
		
		///<summary>
		///	Called once per frame
		///</summary>
		void Update () 
		{

			
		}
		
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		override protected void OnDestroy ()
		{
			//
			base.OnDestroy();
		}
		
		// PUBLIC
		
		
		// PUBLIC STATIC
		/// <summary>
		/// Dos the create spider.
		/// </summary>
		public GameObject doCreateSpider(RoundDataVO aRoundDataVO)
		{


			//POSITION
			float spawnRadius_float 	= _getRadiusFromGameObject(spawnSphereGameObject);
			float attackRadius_float 	= _getRadiusFromGameObject(attackSphereGameObject);
			//
			float spawnAngle_float;
			if (enemyPlacement == EnemyPlacement.DEBUG) {
				spawnAngle_float = 90	; 
			} else {
				spawnAngle_float = Random.Range (0, 360);

				//round to 'space apart' the spawning
				//NOTE: This discourages (but not eliminates) placing overlapping spiders
				spawnAngle_float 			= Mathf.Round (spawnAngle_float/36)*36;
			}


			//
			spawnAngle_float			= Mathf.Deg2Rad*spawnAngle_float;
			float spawnX_float			= Mathf.Cos (spawnAngle_float)*spawnRadius_float;
			float spawnZ_float			= Mathf.Sin (spawnAngle_float)*spawnRadius_float;


			//CREATE ENEMY AND SET THE TARGET IS SHOULD CHASE
			GameObject spider_gameobject = 
				Instantiate (
					spiderPrefabGameObject, 
					new Vector3 (spawnX_float, EnemyUI.DEFAULT_Y_POSITION, spawnZ_float), Quaternion.identity
				) as GameObject; 
			//
			spider_gameobject.transform.parent = enemyParentGameObject.transform;

			//SET BASIC PARAMETERS FOR BEHAVIOR
			float enemyHealth_float = aRoundDataVO.enemyHealthRange.getRandomIntValueWithinRange(); //use int
			float enemySpeed_float = aRoundDataVO.enemySpeedRange.getRandomFloatValueWithinRange(); //use float
			spider_gameobject.GetComponent<EnemyUI>().setParameters (
				targetGameObject, 
				attackRadius_float, 
				enemyHealth_float,
				enemySpeed_float);

			//TODO: ENSURE A SPIDER IS NOT SPAWNED ON TOP OF AN OTHER ONE
			//TODO: ENSURE SPIDERS COME FROM 'ALL AROUND' WITHOUT 'REPEATING TOO MUCH'

			//
			return spider_gameobject;
		}
		
		// PRIVATE
		/// <summary>
		/// _gets the radius from game object.
		/// </summary>
		/// <returns>The radius from game object.</returns>
		/// <param name="aGameObject">A game object.</param>
		private float _getRadiusFromGameObject (GameObject aGameObject)
		{
			//TODO, ADJUST FOR THE SIZE OF THE SPIDER ITSELF
			return aGameObject.GetComponent<SphereCollider>().radius*aGameObject.transform.lossyScale.x;
		}

		// PRIVATE STATIC
		
		// PRIVATE COROUTINE
		
		// PRIVATE INVOKE
		
		//--------------------------------------
		//  Events 
		//		(This is a loose term for -- handling incoming messaging)
		//
		//--------------------------------------
		
		

	}
}

