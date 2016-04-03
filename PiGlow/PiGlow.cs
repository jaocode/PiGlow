using System;
using RPi.I2C.Net;

namespace RPi.PiGlow
{
	public class PiGlow
	{
		private const int PIGLOW_ADDRESS = 0x54;       //the gpio adress
		private const byte CMD_ENABLE_OUTPUT = 0x00;   //command to enable all outputs
		private const byte CMD_ENABLE_LEDS = 0x13;     //enable the LEDs
		private const byte CMD_SET_PWM_VALUES = 0x01;  //command address that sets the brightness values
		private const byte CMD_UPDATE = 0x16;          //command that causes the hardware board to update its LED values

		private static byte[] _whites = {0x0A, 0x0D, 0x0B};
		private static byte[] _blues  = {0x05, 0x0F, 0x0C};
		private static byte[] _greens = {0x06, 0x04, 0x0E};
		private static byte[] _yellows = {0x09, 0x03, 0x10};
		private static byte[] _oranges = {0x08, 0x02, 0x11};
		private static byte[] _reds = {0x07, 0x01, 0x12};

		// Arms from left to right
		private static byte[] _arm0 = {_whites[0], _blues[0], _greens[0], _yellows[0], _oranges[0], _reds[0]};
		private static byte[] _arm1 = {_whites[2], _blues[2], _greens[2], _yellows[2], _oranges[2], _reds[2]};
		private static byte[] _arm2 = {_whites[1], _blues[1], _greens[1], _yellows[1], _oranges[1], _reds[1]};

		private static byte[][] _arms = {_arm0, _arm1, _arm2};

		public static byte[] Whites { get {return _whites;} }
		public static byte[] Blues { get {return _blues;} }
		public static byte[] Greens { get {return _greens;} }
		public static byte[] Yellows { get {return _yellows;} }
		public static byte[] Oranges { get {return _oranges;} }
		public static byte[] Reds { get {return _reds;} }

//		public static byte[] ARM0 {get {return _arm0;}}
//		public static byte[] ARM1 {get {return _arm1;}}
//		public static byte[] ARM2 {get {return _arm2;}}
		public static byte[][] Arms = _arms;

		private I2CBus _i2cBus;

		public enum Revision {First = 1, Second = 2, Thrid = 3};
		private static Revision _revision = Revision.Thrid;

		private const string BUS_0 = "/dev/i2c-0";
		private const string BUS_1 = "/dev/i2c-1";

		public static Revision RPiVersion
		{
			set
			{
				_revision = value;
			}
			get
			{
				return _revision;
			}
		}

		private static PiGlow _instance;

		public static PiGlow Instance
		{
			get 
			{
				if (_instance == null)
				{
					_instance = new PiGlow (_revision);
				}

				return _instance;
			}
		}

		private void Init ()
		{
			_i2cBus.WriteBytes (PIGLOW_ADDRESS, new byte[] {0x00, 0x01});

			_i2cBus.WriteCommand (PIGLOW_ADDRESS, 0x13, 0xff);
			_i2cBus.WriteCommand (PIGLOW_ADDRESS, 0x14, 0xff);
			_i2cBus.WriteCommand (PIGLOW_ADDRESS, 0x15, 0xff);
		}

		private PiGlow (Revision revision)
		{
			string bus = BUS_0;

			switch (revision) 
			{
				case Revision.First:
					bus = BUS_0;
					break;
				case Revision.Second:
				case Revision.Thrid:
					bus = BUS_1;
					break;
				default:
					throw new Exception ("Unknown Revision");
			}

			_i2cBus = I2CBus.Open (bus);
			Init ();
		}

		private byte TrueIntensity (float intensityPercent)
		{
			return Convert.ToByte (intensityPercent * 255f);
		}

		public void SetLED (byte led, float intensity)
		{
			_i2cBus.WriteCommand (PIGLOW_ADDRESS, led, TrueIntensity(intensity));
			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}

		public void SetLEDs (byte [] leds, float intensity)
		{
			for (int i = 0; i < leds.Length; i++)
				_i2cBus.WriteCommand (PIGLOW_ADDRESS, leds[i], TrueIntensity(intensity));
			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}

		public void SetWhites (float intensity)
		{
			foreach (byte led in _whites)
				_i2cBus.WriteCommand (PIGLOW_ADDRESS, led, TrueIntensity(intensity));

			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}

		public void SetBlues (float intensity)
		{
			foreach (byte led in _blues)
				_i2cBus.WriteCommand (PIGLOW_ADDRESS, led, TrueIntensity(intensity));

			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}
			
		public void SetGreens (float intensity)
		{
			foreach (byte led in _greens)
				_i2cBus.WriteCommand (PIGLOW_ADDRESS, led, TrueIntensity(intensity));

			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}

		public void SetYellows (float intensity)
		{
			foreach (byte led in _yellows)
				_i2cBus.WriteCommand (PIGLOW_ADDRESS, led, TrueIntensity(intensity));

			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}

		public void SetOranges (float intensity)
		{
			foreach (byte led in _oranges)
				_i2cBus.WriteCommand (PIGLOW_ADDRESS, led, TrueIntensity(intensity));

			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}

		public void SetReds (float intensity)
		{
			foreach (byte led in _reds)
				_i2cBus.WriteCommand (PIGLOW_ADDRESS, led, TrueIntensity(intensity));

			_i2cBus.WriteCommand (PIGLOW_ADDRESS, CMD_UPDATE, 0xff);
		}

		public void AllOff ()
		{ 
			SetWhites (0);
			SetBlues (0);
			SetGreens (0);
			SetYellows (0);
			SetOranges (0);
			SetReds (0);
		}
	}
}

