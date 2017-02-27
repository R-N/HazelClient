﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public struct int2{
	public int x;
	public int y;
	public int2(int x, int y){
		this.x = x;
		this.y = y;
	}
	public static int2 operator +(int2 a, int2 b){
		return new int2 (a.x + b.x, a.y + b.y);
	}
	public static int2 operator -(int2 a, int2 b){
		return new int2 (a.x - b.x, a.y - b.y);
	}
	public static bool operator ==(int2 a, int2 b){
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator !=(int2 a, int2 b){
		return a.x != b.x && a.y != b.y;
	}
}
public struct int3{
	public int x;
	public int y;
	public int z;
	public int3(int x, int y, int z){
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public int2 xy {
		get {
			return new int2 (x, y);
		}
	}

	public int2 xz {
		get {
			return new int2 (x, z);
		}
	}

	public static int3 operator +(int3 a, int3 b){
		return new int3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}
	public static int3 operator -(int3 a, int3 b){
		return new int3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}
	public static bool operator ==(int3 a, int3 b){
		return a.x == b.x && a.y == b.y && a.z == b.z;
	}
	public static bool operator !=(int3 a, int3 b){
		return a.x != b.x || a.y != b.y || a.z != b.z;
	}
}

public struct RoomInfo{
	public int number;
	public string name;
	public bool hasPassword;
	public bool playing;
	public int maxPlayers;
	public int curPlayers;
}

public class HazelWriter{
	public const byte one = 1;
	public const byte zero = 0;
	public byte[] bytes;
	public HazelWriter(){
		bytes = new byte[]{};
	}
	public HazelWriter(byte[] nb){
		bytes = nb;
	}


	public void WriteByte(byte input){
		bytes = bytes.Concat(new byte[]{ input }).ToArray();
	}

	public void Write(byte input){
		bytes = bytes.Concat(new byte[]{ input }).ToArray();
	}

	public void Write(bool input){
		bytes = bytes.Concat(new byte[]{ input ? one : zero }).ToArray();
	}
	public void Write(short input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(ushort input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(int input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(uint input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(long input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(ulong input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(float input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(double input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(char input){
		bytes = bytes.Concat(BitConverter.GetBytes(input)).ToArray ();
	}
	public void Write(byte[] input){
		bytes = bytes.Concat (input).ToArray ();
	}
	public void Write(string input){
		if (string.IsNullOrEmpty (input)) {
			Write (0);
			return;
		}
		int len = input.Length;
		int len3 = len * 2;
		byte[] temp5 = new byte[len3];
		System.Buffer.BlockCopy(input.ToCharArray(), 0, temp5, 0, len3);

		Write (len);
		bytes = bytes.Concat(temp5).ToArray ();
	}

	public void Write (Vector2 input){
		byte[] temp = new byte[8];
		byte[] temp2;

		temp2 = BitConverter.GetBytes (input.x);
		temp [0] = temp2 [0];
		temp [1] = temp2 [1];
		temp [2] = temp2 [2];
		temp [3] = temp2 [3];
		temp2 = BitConverter.GetBytes (input.y);
		temp [4] = temp2 [0];
		temp [5] = temp2 [1];
		temp [6] = temp2 [2];
		temp [7] = temp2 [3];

		bytes = bytes.Concat(temp).ToArray ();
	}
	public void Write (int2 input){
		byte[] temp = new byte[8];
		byte[] temp2;

		temp2 = BitConverter.GetBytes (input.x);
		temp [0] = temp2 [0];
		temp [1] = temp2 [1];
		temp [2] = temp2 [2];
		temp [3] = temp2 [3];
		temp2 = BitConverter.GetBytes (input.y);
		temp [4] = temp2 [0];
		temp [5] = temp2 [1];
		temp [6] = temp2 [2];
		temp [7] = temp2 [3];

		bytes = bytes.Concat(temp).ToArray ();
	}
	public void Write (Vector3 input){
		byte[] temp = new byte[12];
		byte[] temp2;

		temp2 = BitConverter.GetBytes (input.x);
		temp [0] = temp2 [0];
		temp [1] = temp2 [1];
		temp [2] = temp2 [2];
		temp [3] = temp2 [3];
		temp2 = BitConverter.GetBytes (input.y);
		temp [4] = temp2 [0];
		temp [5] = temp2 [1];
		temp [6] = temp2 [2];
		temp [7] = temp2 [3];
		temp2 = BitConverter.GetBytes (input.z);
		temp [8] = temp2 [0];
		temp [9] = temp2 [1];
		temp [10] = temp2 [2];
		temp [11] = temp2 [3];

		bytes = bytes.Concat(temp).ToArray ();
	}
	public void Write (Quaternion input){
		byte[] temp = new byte[16];
		byte[] temp2;

		temp2 = BitConverter.GetBytes (input.x);
		temp [0] = temp2 [0];
		temp [1] = temp2 [1];
		temp [2] = temp2 [2];
		temp [3] = temp2 [3];
		temp2 = BitConverter.GetBytes (input.y);
		temp [4] = temp2 [0];
		temp [5] = temp2 [1];
		temp [6] = temp2 [2];
		temp [7] = temp2 [3];
		temp2 = BitConverter.GetBytes (input.z);
		temp [8] = temp2 [0];
		temp [9] = temp2 [1];
		temp [10] = temp2 [2];
		temp [11] = temp2 [3];
		temp2 = BitConverter.GetBytes (input.w);
		temp [12] = temp2 [0];
		temp [13] = temp2 [1];
		temp [14] = temp2 [2];
		temp [15] = temp2 [3];

		bytes = bytes.Concat(temp).ToArray ();
	}


	public void Write  (Matrix4x4 input){

		bytes = bytes.Concat( BitConverter.GetBytes (input.m00))
			.Concat (BitConverter.GetBytes (input.m01))
			.Concat (BitConverter.GetBytes (input.m02))
			.Concat (BitConverter.GetBytes (input.m03))
			.Concat (BitConverter.GetBytes (input.m10))
			.Concat (BitConverter.GetBytes (input.m11))
			.Concat (BitConverter.GetBytes (input.m12))
			.Concat (BitConverter.GetBytes (input.m13))
			.Concat (BitConverter.GetBytes (input.m20))
			.Concat (BitConverter.GetBytes (input.m21))
			.Concat (BitConverter.GetBytes (input.m22))
			.Concat (BitConverter.GetBytes (input.m23))
			.Concat (BitConverter.GetBytes (input.m30))
			.Concat (BitConverter.GetBytes (input.m31))
			.Concat (BitConverter.GetBytes (input.m32))
			.Concat (BitConverter.GetBytes (input.m33))
			.ToArray ();
	}

	public void Write(RoomInfo input){
		Write (input.number);
		Write (input.name);
		Write (input.hasPassword);
		Write (input.playing);
		Write (input.maxPlayers);
		Write (input.curPlayers);
	}
}

public class HazelReader{
	public const byte one = 1;
	public const byte zero = 0;
	public byte[] bytes;
	public int pos = 0;
	public int length;
	public HazelReader(byte[] nb){
		bytes = nb;
		length = nb.Length;
	}

	public bool ReadBool (){
		return bytes [pos++] != zero;
	}

	public byte ReadByte(){
		return bytes [pos++];
	}

	public short ReadShort(){
		short ret = BitConverter.ToInt16 (bytes, pos);
		pos += sizeof(short);
		return ret;
	}
	public ushort ReadUShort(){
		ushort ret = BitConverter.ToUInt16 (bytes, pos);
		pos += sizeof(ushort);
		return ret;
	}
	public int ReadInt(){
		int ret = BitConverter.ToInt32 (bytes, pos);
		pos += sizeof(int);
		return ret;
	}
	public uint ReadUInt(){
		uint ret = BitConverter.ToUInt32 (bytes, pos);
		pos += sizeof(uint);
		return ret;
	}
	public long ReadLong(){
		long ret = BitConverter.ToInt64 (bytes, pos);
		pos += sizeof(long);
		return ret;
	}
	public ulong ReadULong(){
		ulong ret = BitConverter.ToUInt64 (bytes, pos);
		pos += sizeof(ulong);
		return ret;
	}
	public float ReadFloat(){
		float ret = BitConverter.ToSingle (bytes, pos);
		pos += 4;
		return ret;
	}
	public double ReadDouble(){
		double ret = BitConverter.ToDouble (bytes, pos);
		pos += sizeof(double);
		return ret;
	}
	public char ReadChar(){
		char ret = BitConverter.ToChar (bytes, pos);
		pos += 2;
		return ret;
	}
	public string ReadString(){
		if (pos >= bytes.Length)
			return null;
		int len = ReadInt ();
		int len2 = len * 2;
		char[] chars = new char[len];
		System.Buffer.BlockCopy(bytes, pos, chars, 0, len2);
		pos += len2;
		return new string(chars);
	}

	public Vector2 ReadVector2(){
		return new Vector2 (ReadFloat (), ReadFloat ());
	}
	public int2 ReadInt2(){
		return new int2 (ReadInt (), ReadInt ());
	}
	public Vector3 ReadVector3(){
		return new Vector3 (ReadFloat (), ReadFloat (), ReadFloat());
	}
	public Quaternion ReadQuaternion(){
		return new Quaternion (ReadFloat (), ReadFloat (), ReadFloat (), ReadFloat ());
	}
	public Matrix4x4 ReadMatrix4x4(){
		Matrix4x4 ret = new Matrix4x4 ();
		ret.m00 = ReadFloat ();
		ret.m01 = ReadFloat ();
		ret.m02 = ReadFloat ();
		ret.m03 = ReadFloat ();
		ret.m10 = ReadFloat ();
		ret.m11 = ReadFloat ();
		ret.m12 = ReadFloat ();
		ret.m13 = ReadFloat ();
		ret.m20 = ReadFloat ();
		ret.m21 = ReadFloat ();
		ret.m22 = ReadFloat ();
		ret.m23 = ReadFloat ();
		ret.m30 = ReadFloat ();
		ret.m31 = ReadFloat ();
		ret.m32 = ReadFloat ();
		ret.m33 = ReadFloat ();
		return ret;
	}

	public RoomInfo ReadRoomInfo(){
		return new RoomInfo () {
			number = ReadInt (),
			name = ReadString (),
			hasPassword = ReadBool (),
			playing = ReadBool (),
			maxPlayers = ReadInt (),
			curPlayers = ReadInt ()
		};
	}


	public bool ReadBool (int pos){
		return bytes [pos] != zero;
	}

	public byte ReadByte(int pos){
		return bytes [pos];
	}

	public short ReadShort(int pos){
		return BitConverter.ToInt16 (bytes, pos);
	}
	public ushort ReadUShort(int pos){
		return BitConverter.ToUInt16 (bytes, pos);
	}
	public int ReadInt(int pos){
		return BitConverter.ToInt32 (bytes, pos);
	}
	public uint ReadUInt(int pos){
		uint ret = BitConverter.ToUInt32 (bytes, pos);
		pos += sizeof(uint);
		return ret;
	}
	public long ReadLong(int pos){
		return BitConverter.ToInt64 (bytes, pos);
	}
	public ulong ReadULong(int pos){
		return BitConverter.ToUInt64 (bytes, pos);
	}
	public float ReadFloat(int pos){
		return BitConverter.ToSingle (bytes, pos);
	}
	public double ReadDouble(int pos){
		return BitConverter.ToDouble (bytes, pos);
	}
	public char ReadChar(int pos){
		return BitConverter.ToChar (bytes, pos);
	}
	public string ReadString(int pos){
		if (pos >= bytes.Length)
			return null;
		int len = ReadInt (pos);
		int len2 = len * 2;
		char[] chars = new char[len];
		System.Buffer.BlockCopy(bytes, pos + 4, chars, 0, len2);
		return new string(chars);
	}

	public Vector2 ReadVector2(int pos){
		return new Vector2 (ReadFloat (pos), ReadFloat (pos + 4));
	}
	public int2 ReadInt2(int pos){
		return new int2 (ReadInt (pos), ReadInt (pos + 4));
	}
	public Vector3 ReadVector3(int pos){
		return new Vector3 (ReadFloat (pos), ReadFloat (pos + 4), ReadFloat(pos + 8));
	}
	public Quaternion ReadQuaternion(int pos){
		return new Quaternion (ReadFloat (pos), ReadFloat (pos + 4), ReadFloat (pos + 8), ReadFloat (pos + 12));
	}

	public Matrix4x4 ReadMatrix4x4(int pos){
		Matrix4x4 ret = new Matrix4x4 ();
		ret.m00 = ReadFloat (pos);
		ret.m01 = ReadFloat (pos + 4);
		ret.m02 = ReadFloat (pos + 8);
		ret.m03 = ReadFloat (pos + 12);
		ret.m10 = ReadFloat (pos + 16);
		ret.m11 = ReadFloat (pos + 20);
		ret.m12 = ReadFloat (pos + 24);
		ret.m13 = ReadFloat (pos + 28);
		ret.m20 = ReadFloat (pos + 32);
		ret.m21 = ReadFloat (pos + 36);
		ret.m22 = ReadFloat (pos + 40);
		ret.m23 = ReadFloat (pos + 44);
		ret.m30 = ReadFloat (pos + 48);
		ret.m31 = ReadFloat (pos + 52);
		ret.m32 = ReadFloat (pos + 56);
		ret.m33 = ReadFloat (pos + 60);
		return ret;
	}
}


