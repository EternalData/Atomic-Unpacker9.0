using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomic_UnPacker
{
    class AntiDe4DotRemover
    {
        public static void RemoveAnti(ModuleDefMD module)
        {
			for (int i = 0; i < module.Types.Count; i++)
			{
				TypeDef typeDef = module.Types[i];
				bool hasInterfaces = typeDef.HasInterfaces;
				bool flag = hasInterfaces;
				if (flag)
				{
					for (int j = 0; j < typeDef.Interfaces.Count; j++)
					{
						bool flag2 = typeDef.Interfaces[j].Interface != null;
						bool flag3 = flag2;
						bool flag4 = flag3;
						if (flag4)
						{
							bool flag5 = typeDef.Interfaces[j].Interface.Name.Contains(typeDef.Name) || typeDef.Name.Contains(typeDef.Interfaces[j].Interface.Name);
							bool flag6 = flag5;
							if (flag6)
							{
								module.Types.RemoveAt(i);
								Program.countofths++;
							}
						}
					}
				}
			}
			foreach (TypeDef typeDef2 in from t in module.Types.ToList<TypeDef>()
										 where t.HasInterfaces
										 select t)
			{
				for (int k = 0; k < typeDef2.Interfaces.Count; k++)
				{
					bool flag7 = typeDef2.Interfaces[k].Interface.Name.Contains(typeDef2.Name) || typeDef2.Name.Contains(typeDef2.Interfaces[k].Interface.Name);
					if (flag7)
					{
						module.Types.Remove(typeDef2);
						Program.countofths++;
					}
				}
			}
			List<string> list = new List<string>
			{
				"DotNetPatcherObfuscatorAttribute",
				"DotNetPatcherPackerAttribute",
				"DotfuscatorAttribute",
				"ObfuscatedByGoliath",
				"dotNetProtector",
				"PoweredByAttribute",
				"AssemblyInfoAttribute",
				"BabelAttribute",
				"CryptoObfuscator.ProtectedWithCryptoObfuscatorAttribute",
				"Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode",
				"NineRays.Obfuscator.Evaluation",
				"YanoAttribute",
				"SmartAssembly.Attributes.PoweredByAttribute",
				"NetGuard",
				"SecureTeam.Attributes.ObfuscatedByCliSecureAttribute",
				"Reactor",
				"ZYXDNGuarder",
				"CryptoObfuscator"
			};
			foreach (TypeDef typeDef3 in module.Types.ToList<TypeDef>())
			{
				bool flag8 = list.Contains(typeDef3.Name);
				if (flag8)
				{
					module.Types.Remove(typeDef3);
					Program.countofths++;
				}
			}
			ModuleDef a_ = module;
			for (int l = 0; l < module.CustomAttributes.Count; l++)
			{
				CustomAttribute customAttribute = module.CustomAttributes[l];
				bool flag9 = customAttribute == null;
				bool flag10 = !flag9;
				if (flag10)
				{
					TypeDef typeDef4 = customAttribute.AttributeType.ResolveTypeDef();
					bool flag11 = typeDef4 == null;
					bool flag12 = !flag11;
					if (flag12)
					{
						bool flag13 = typeDef4.Name == "ConfusedByAttribute";
						bool flag14 = flag13;
						if (flag14)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag15 = typeDef4.Name == "ZYXDNGuarder";
						bool flag16 = flag15;
						if (flag16)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag17 = typeDef4.Name == "YanoAttribute";
						bool flag18 = flag17;
						if (flag18)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag19 = typeDef4.Name == "Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode";
						bool flag20 = flag19;
						if (flag20)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag21 = typeDef4.Name == "SmartAssembly.Attributes.PoweredByAttribute";
						bool flag22 = flag21;
						if (flag22)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag23 = typeDef4.Name == "SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute";
						bool flag24 = flag23;
						if (flag24)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag25 = typeDef4.Name == "ObfuscatedByGoliath";
						bool flag26 = flag25;
						if (flag26)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag27 = typeDef4.Name == "NineRays.Obfuscator.Evaluation";
						bool flag28 = flag27;
						if (flag28)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag29 = typeDef4.Name == "EMyPID_8234_";
						bool flag30 = flag29;
						if (flag30)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag31 = typeDef4.Name == "DotfuscatorAttribute";
						bool flag32 = flag31;
						if (flag32)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag33 = typeDef4.Name == "CryptoObfuscator.ProtectedWithCryptoObfuscatorAttribute";
						bool flag34 = flag33;
						if (flag34)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag35 = typeDef4.Name == "BabelObfuscatorAttribute";
						bool flag36 = flag35;
						if (flag36)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag37 = typeDef4.Name == ".NETGuard";
						bool flag38 = flag37;
						if (flag38)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag39 = typeDef4.Name == "OrangeHeapAttribute";
						bool flag40 = flag39;
						if (flag40)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag41 = typeDef4.Name == "WTF";
						bool flag42 = flag41;
						if (flag42)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag43 = typeDef4.Name == "<ObfuscatedByDotNetPatcher>";
						bool flag44 = flag43;
						if (flag44)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag45 = typeDef4.Name == "SecureTeam.Attributes.ObfuscatedByCliSecureAttribute";
						bool flag46 = flag45;
						if (flag46)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag47 = typeDef4.Name == "SmartAssembly.Attributes.PoweredByAttribute";
						bool flag48 = flag47;
						if (flag48)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag49 = typeDef4.Name == "Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode";
						bool flag50 = flag49;
						if (flag50)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag51 = typeDef4.Name == "OiCuntJollyGoodDayYeHavin_____________________________________________________";
						if (flag51)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag52 = typeDef4.Name == "ProtectedWithCryptoObfuscatorAttribute";
						if (flag52)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag53 = typeDef4.Name == "NetGuard";
						if (flag53)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag54 = typeDef4.Name == "ZYXDNGuarder";
						if (flag54)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag55 = typeDef4.Name == "DotfuscatorAttribute";
						if (flag55)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						bool flag56 = typeDef4.Name == "SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute";
						if (flag56)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
					}
				}
			}
		}
    }
}
