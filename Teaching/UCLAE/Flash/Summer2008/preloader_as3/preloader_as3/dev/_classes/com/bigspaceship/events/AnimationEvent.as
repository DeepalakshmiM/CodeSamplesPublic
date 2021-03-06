/*
Copyright (C) 2006 Big Spaceship, LLC

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

To contact Big Spaceship, email info@bigspaceship.com or write to us at 45 Main Street #716, Brooklyn, NY, 11201.
*/
package com.bigspaceship.events{
	
	import flash.events.Event;
	
	public class AnimationEvent extends Event{
		public static const ANIMATE_IN        : String = "animateIn";
		public static const ANIMATE_OUT       : String = "animateOut";
		public static const ANIMATE_START     : String = "animateBegin";
		public static const ANIMATE_IN_START  : String = "animateInStart";
		public static const ANIMATE_OUT_START : String = "animateOutStart";
		public static const ANIMATE_FINISH    : String = "animateFinish";
		public static const ANIMATE_CANCEL    : String = "animateCancel";
		
		
		public var clip : *;
		
		public function AnimationEvent($type:String, $clip:* = null){
			super($type);
			clip = $clip;
		};

		public override function clone():Event{
			return new AnimationEvent(type, clip);
		};
	};
};