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
						bool flag2 = 
						bool flag3 = flag2;
						bool flag4 = flag3;
						if (typeDef.Interfaces[j].Interface != null)
						{
							
							if (typeDef.Interfaces[j].Interface.Name.Contains(typeDef.Name) || typeDef.Name.Contains(typeDef.Interfaces[j].Interface.Name);)
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
					
					if (typeDef2.Interfaces[k].Interface.Name.Contains(typeDef2.Name) || typeDef2.Name.Contains(typeDef2.Interfaces[k].Interface.Name))
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
						
						if (typeDef4.Name == "ConfusedByAttribute")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if ( typeDef4.Name == "ZYXDNGuarder")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "YanoAttribute")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "SmartAssembly.Attributes.PoweredByAttribute")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute")
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
						
						if (typeDef4.Name == "OiCuntJollyGoodDayYeHavin_____________________________________________________")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "ProtectedWithCryptoObfuscatorAttribute";)
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "NetGuard")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "ZYXDNGuarder")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "DotfuscatorAttribute")
						{
							Program.lista(a_).Remove(typeDef4);
							Program.countofths++;
						}
						
						if (typeDef4.Name == "SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute")
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
