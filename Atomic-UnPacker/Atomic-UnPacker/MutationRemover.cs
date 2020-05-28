using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Atomic_UnPacker
{
    public class MutationRemover
    {
		public static void Execute(ModuleDefMD m)
        {
			sizeofCleaner(m);
			EmptyTypesCleaner(m);
			ParseRemover(m);
			DecimalCompare(m);
			stringLengthFixer(m);
			CallMathStep1(m);
		}
		public static void stringLengthFixer(ModuleDefMD module)
		{
			int c = 0;
			foreach (TypeDef type in module.GetTypes())
			{
				foreach (MethodDef method in type.Methods)
				{
					bool hasBody = method.HasBody;
					if (hasBody)
					{
						for (int i = 0; i < method.Body.Instructions.Count; i++)
						{
							bool flag = method.Body.Instructions[i].OpCode == OpCodes.Call && method.Body.Instructions[i].Operand.ToString().Contains("get_Length") && method.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldstr;
							if (flag)
							{
								string text = method.Body.Instructions[i - 1].Operand.ToString();
								int length = text.Length;
								Console.WriteLine("[!] {0} Length is {1}", text, length);
								method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
								method.Body.Instructions[i].Operand = length;
								method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
								c++;
							}
						}
					}
				}
			}
		}
		public static void DecimalCompare(ModuleDefMD module)
		{
			int compared = 0;
			foreach (TypeDef type in module.GetTypes())
			{
				foreach (MethodDef method in type.Methods)
				{
					bool hasBody = method.HasBody;
					if (hasBody)
					{
						for (int i = 0; i < method.Body.Instructions.Count; i++)
						{
							bool flag = method.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && method.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && method.Body.Instructions[i].Operand.ToString().Contains("Compare") && method.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Newobj && method.Body.Instructions[i - 2].IsLdcI4() && method.Body.Instructions[i - 4].IsLdcI4();
							if (flag)
							{
								int ldcI4Value = method.Body.Instructions[i - 4].GetLdcI4Value();
								int ldcI4Value2 = method.Body.Instructions[i - 2].GetLdcI4Value();
								int num = decimal.Compare(ldcI4Value, ldcI4Value2);
								Console.WriteLine("[!] Decimal.Compare({0},{1}) is {2}", ldcI4Value, ldcI4Value2, num);
								method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
								method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
								method.Body.Instructions[i - 2].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
								method.Body.Instructions[i - 3].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
								method.Body.Instructions[i - 4].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
								method.Body.Instructions[i - 4].Operand = num;
								Console.WriteLine(string.Format("Fixed Decimal Compare on {0} at offset: {1}", method.Name, method.Body.Instructions[i].Offset), ConsoleColor.Blue);
								compared++;
							}
						}
					}
				}
			}
		}
		public static void sizeofCleaner(ModuleDefMD module)
		{
			foreach (TypeDef type in module.Types)
				foreach (MethodDef method in type.Methods)
				{
					if (!method.HasBody) continue;
					for (int index = 0; index < method.Body.Instructions.Count; ++index)
						if (method.Body.Instructions[index].OpCode == OpCodes.Sizeof)
						{
							switch ((method.Body.Instructions[index].Operand as ITypeDefOrRef).ToString())
							{
								case "System.Boolean":
									method.Body.Instructions[index].OpCode = OpCodes.Ldc_I4_1;
									break;
								case "System.Byte":
									method.Body.Instructions[index].OpCode = OpCodes.Ldc_I4_1;
									break;
								case "System.Decimal":
									method.Body.Instructions[index] = OpCodes.Ldc_I4.ToInstruction(16);
									break;
								case "System.Double":
								case "System.Int64":
								case "System.UInt64":
									method.Body.Instructions[index].OpCode = OpCodes.Ldc_I4_8;
									break;
								case "System.Guid":
									method.Body.Instructions[index] = OpCodes.Ldc_I4.ToInstruction(16);
									break;
								case "System.Int16":
								case "System.UInt16":
									method.Body.Instructions[index].OpCode = OpCodes.Ldc_I4_2;
									break;
								case "System.Int32":
								case "System.Single":
								case "System.UInt32":
									method.Body.Instructions[index].OpCode = OpCodes.Ldc_I4_4;
									break;
								case "System.SByte":
									method.Body.Instructions[index].OpCode = OpCodes.Ldc_I4_1;
									break;
							}
							if (!method.Body.Instructions[index].IsLdcI4())
							{
								int sizeOf = getSizeOfValue(method.Body.Instructions[index].Operand.ToString());
								if (sizeOf != 0)
								{
									method.Body.Instructions[index] = OpCodes.Ldc_I4.ToInstruction(sizeOf);
								}
							}
						}
				}
		}
		private static int getSizeOfValue(string operand)
		{
			int result;
			try
			{
				Type type = Type.GetType(operand);
				bool flag = type != null;
				if (flag)
				{
					result = Marshal.SizeOf(type);
				}
				else
				{
					result = 0;
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}
		public static void EmptyTypesCleaner(ModuleDefMD module)
		{
			foreach (TypeDef type in module.GetTypes())
			{
				foreach (MethodDef method in type.Methods)
				{
					bool hasBody = method.HasBody;
					if (hasBody)
					{
						for (int i = 0; i < method.Body.Instructions.Count; i++)
						{
							bool flag = method.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Ldsfld && method.Body.Instructions[i].Operand.ToString().Contains("::EmptyTypes") && method.Body.Instructions[i + 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldlen;
							if (flag)
							{
								method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4_0;
								method.Body.Instructions[i + 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
								Console.WriteLine(string.Format("Fixed the empty types in method: {0} at offset: {1}", method.Name, method.Body.Instructions[i].Offset), ConsoleColor.Blue);
							}
						}
					}
				}
			}
		}
		public static void ParseRemover(ModuleDefMD modulee)
		{
			int parse = 0;
			foreach (TypeDef type in modulee.GetTypes())
			{
				foreach (MethodDef method in type.Methods)
				{
					bool hasBody = method.HasBody;
					if (hasBody)
					{
						for (int i = 1; i < method.Body.Instructions.Count - 1; i++)
						{
							bool flag = method.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && method.Body.Instructions[i].Operand.ToString().Contains("Parse") && method.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldstr;
							if (flag)
							{
								MemberRef Parse = (MemberRef)method.Body.Instructions[i].Operand;
								bool flag2 = Parse.DeclaringType.Name.Contains("Int32");
								if (flag2)
								{
									int result = int.Parse(method.Body.Instructions[i - 1].Operand.ToString());
									method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
									method.Body.Instructions[i].Operand = result;
									method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
									parse++;
								}
								else
								{
									bool flag3 = Parse.DeclaringType.Name.Contains("Single");
									if (flag3)
									{
										float result2 = float.Parse(method.Body.Instructions[i - 1].Operand.ToString());
										method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_R4;
										method.Body.Instructions[i].Operand = result2;
										method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
										parse++;
									}
									else
									{
										bool flag4 = Parse.DeclaringType.Name.Contains("Int64");
										if (flag4)
										{
											long result3 = long.Parse(method.Body.Instructions[i - 1].Operand.ToString());
											method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I8;
											method.Body.Instructions[i].Operand = result3;
											method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
											parse++;
										}
										else
										{
											bool flag5 = Parse.DeclaringType.Name.Contains("Double");
											if (flag5)
											{
												double result4 = double.Parse(method.Body.Instructions[i - 1].Operand.ToString());
												method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_R8;
												method.Body.Instructions[i].Operand = result4;
												method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
												parse++;
											}
											else
											{
												bool flag6 = Parse.DeclaringType.Name.Contains("Decimal");
												if (flag6)
												{
													decimal result5 = decimal.Parse(method.Body.Instructions[i - 1].Operand.ToString());
													method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_R4;
													method.Body.Instructions[i].Operand = (float)result5;
													method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
													parse++;
												}
												else
												{
													bool flag7 = Parse.DeclaringType.Name.Contains("UInt32");
													if (flag7)
													{
														uint result6 = uint.Parse(method.Body.Instructions[i - 1].Operand.ToString());
														method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
														method.Body.Instructions[i].Operand = (int)result6;
														method.Body.Instructions.Add(dnlib.DotNet.Emit.OpCodes.Conv_U4.ToInstruction());
														method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
														parse++;
													}
													else
													{
														bool flag8 = Parse.DeclaringType.Name.Contains("UInt64");
														if (flag8)
														{
															ulong result7 = ulong.Parse(method.Body.Instructions[i - 1].Operand.ToString());
															method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I8;
															method.Body.Instructions[i].Operand = (long)result7;
															method.Body.Instructions.Add(dnlib.DotNet.Emit.OpCodes.Conv_U8.ToInstruction());
															method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
															parse++;
														}
														else
														{
															bool flag9 = Parse.DeclaringType.Name.Contains("Int16");
															if (flag9)
															{
																short result8 = short.Parse(method.Body.Instructions[i - 1].Operand.ToString());
																method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
																method.Body.Instructions[i].Operand = (int)result8;
																method.Body.Instructions.Add(dnlib.DotNet.Emit.OpCodes.Conv_I2.ToInstruction());
																method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
																parse++;
															}
															else
															{
																bool flag10 = Parse.DeclaringType.Name.Contains("UInt16");
																if (flag10)
																{
																	ushort result9 = ushort.Parse(method.Body.Instructions[i - 1].Operand.ToString());
																	method.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
																	method.Body.Instructions[i].Operand = (int)result9;
																	method.Body.Instructions.Add(dnlib.DotNet.Emit.OpCodes.Conv_U2.ToInstruction());
																	method.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
																	parse++;
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		public static void CallMathStep1(ModuleDefMD awef)
		{
			TypeDef[] array = (from x in awef.Types
							   where x.HasMethods
							   select x).ToArray<TypeDef>();
			foreach (TypeDef typeDef in array)
			{
				MethodDef[] array2 = (from x in typeDef.Methods
									  where x.HasBody && x.Body.HasInstructions
									  select x).ToArray<MethodDef>();
				foreach (MethodDef methodDef in array2)
				{
					for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
					{
						bool flag = methodDef.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("System.Math::") && methodDef.Body.Instructions[i].Operand.ToString().Contains("(System.Double)") && methodDef.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_R8;
						if (flag)
						{
							MemberRef memberRef = (MemberRef)methodDef.Body.Instructions[i].Operand;
							MethodBase method = typeof(Math).GetMethod(memberRef.Name, new Type[]
							{
							typeof(double)
							});
							double num = (double)methodDef.Body.Instructions[i - 1].Operand;
							double num2 = (double)method.Invoke(null, new object[]
							{
							num
							});
							methodDef.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_R8;
							methodDef.Body.Instructions[i].Operand = num2;
							methodDef.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							Console.WriteLine(string.Format("{0} : {1}", method, num2));
						}
						bool flag2 = methodDef.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("System.Math::") && methodDef.Body.Instructions[i].Operand.ToString().Contains("(System.Single)") && methodDef.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_R4;
						if (flag2)
						{
							MemberRef memberRef2 = (MemberRef)methodDef.Body.Instructions[i].Operand;
							MethodBase method2 = typeof(Math).GetMethod(memberRef2.Name, new Type[]
							{
							typeof(float)
							});
							float num3 = (float)methodDef.Body.Instructions[i - 1].Operand;
							float num4 = (float)method2.Invoke(null, new object[]
							{
							num3
							});
							methodDef.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_R4;
							methodDef.Body.Instructions[i].Operand = num4;
							methodDef.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							Console.WriteLine(string.Format("{0} : {1}", method2, num4));
						}
						bool flag3 = methodDef.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("System.Math::") && methodDef.Body.Instructions[i].Operand.ToString().Contains("(System.Int32)") && methodDef.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_I4;
						if (flag3)
						{
							MemberRef memberRef3 = (MemberRef)methodDef.Body.Instructions[i].Operand;
							MethodBase method3 = typeof(Math).GetMethod(memberRef3.Name, new Type[]
							{
							typeof(int)
							});
							int num5 = (int)methodDef.Body.Instructions[i - 1].Operand;
							int num6 = (int)method3.Invoke(null, new object[]
							{
							num5
							});
							methodDef.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
							methodDef.Body.Instructions[i].Operand = num6;
							methodDef.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							Console.WriteLine(string.Format("{0} : {1}", method3, num6));
						}
						bool flag4 = methodDef.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("System.Math::") && methodDef.Body.Instructions[i].Operand.ToString().Contains("(System.Double,System.Double)") && methodDef.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_R8 && methodDef.Body.Instructions[i - 2].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_R8;
						if (flag4)
						{
							MemberRef memberRef4 = (MemberRef)methodDef.Body.Instructions[i].Operand;
							MethodBase method4 = typeof(Math).GetMethod(memberRef4.Name, new Type[]
							{
							typeof(double),
							typeof(double)
							});
							double num7 = (double)methodDef.Body.Instructions[i - 1].Operand;
							double num8 = (double)methodDef.Body.Instructions[i - 2].Operand;
							double num9 = (double)method4.Invoke(null, new object[]
							{
							num7,
							num8
							});
							methodDef.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_R8;
							methodDef.Body.Instructions[i].Operand = num9;
							methodDef.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							methodDef.Body.Instructions[i - 2].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							Console.WriteLine(string.Format("{0} : {1}", method4, num9));
						}
						bool flag5 = methodDef.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("System.Math::") && methodDef.Body.Instructions[i].Operand.ToString().Contains("(System.Single,System.Single)") && methodDef.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_R4 && methodDef.Body.Instructions[i - 2].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_R4;
						if (flag5)
						{
							MemberRef memberRef5 = (MemberRef)methodDef.Body.Instructions[i].Operand;
							MethodBase method5 = typeof(Math).GetMethod(memberRef5.Name, new Type[]
							{
							typeof(float),
							typeof(float)
							});
							float num10 = (float)methodDef.Body.Instructions[i - 1].Operand;
							float num11 = (float)methodDef.Body.Instructions[i - 2].Operand;
							float num12 = (float)method5.Invoke(null, new object[]
							{
							num10,
							num11
							});
							methodDef.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_R4;
							methodDef.Body.Instructions[i].Operand = num12;
							methodDef.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							methodDef.Body.Instructions[i - 2].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							Console.WriteLine(string.Format("{0} : {1}", method5, num12));
						}
						bool flag6 = methodDef.Body.Instructions[i].OpCode == dnlib.DotNet.Emit.OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("System.Math::") && methodDef.Body.Instructions[i].Operand.ToString().Contains("(System.Int32,System.Int32)") && methodDef.Body.Instructions[i - 1].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_I4 && methodDef.Body.Instructions[i - 2].OpCode == dnlib.DotNet.Emit.OpCodes.Ldc_I4;
						if (flag6)
						{
							MemberRef memberRef6 = (MemberRef)methodDef.Body.Instructions[i].Operand;
							MethodBase method6 = typeof(Math).GetMethod(memberRef6.Name, new Type[]
							{
							typeof(int),
							typeof(int)
							});
							int num13 = (int)methodDef.Body.Instructions[i - 1].Operand;
							int num14 = (int)methodDef.Body.Instructions[i - 2].Operand;
							int num15 = (int)method6.Invoke(null, new object[]
							{
							num13,
							num14
							});
							methodDef.Body.Instructions[i].OpCode = dnlib.DotNet.Emit.OpCodes.Ldc_I4;
							methodDef.Body.Instructions[i].Operand = num15;
							methodDef.Body.Instructions[i - 1].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							methodDef.Body.Instructions[i - 2].OpCode = dnlib.DotNet.Emit.OpCodes.Nop;
							Console.WriteLine(string.Format("{0} : {1}", method6, num15));
						}
					}
				}
			}
		}
	}
}
