using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = System.Random;

[Serializable]
public class Description
{
    public List<string> origin;
    public List<string> civilian;
    public List<string> mission_command;
    public List<string> enemy_desc;
    public List<string> color;
    public List<string> phys;
    public List<string> star;
    public List<string> color_shout;
    public List<string> rd;
    public List<string> rd_phys;
    public List<string> rd_abi;
    public List<string> rg;
    public List<string> rg_phys;
    public List<string> rg_abi;
    public List<string> yd;
    public List<string> yd_phys;
    public List<string> yd_abi;

    public Description(List<string> origin, List<string> civilian, List<string> mission_command, List<string> enemy_desc, 
        List<string> color, List<string> phys, List<string> star, List<string> color_shout, List<string> rd, List<string> rd_phys, 
        List<string> rd_abi, List<string> rg, List<string> rg_phys, List<string> rg_abi, List<string> yd, 
        List<string> yd_phys, List<string> yd_abi)
    {
        this.origin = origin;
        this.civilian = civilian;
        this.mission_command = mission_command;
        this.enemy_desc = enemy_desc;
        this.color = color;
        this.phys = phys;
        this.star = star;
        this.color_shout = color_shout;
        this.rd= rd;
        this.rd_phys = rd_phys;
        this.rd_abi = rd_abi;
        this.rg = rg;
        this.rg_phys = rg_phys;
        this.rg_abi = rg_abi;
        this.yd = yd;
        this.yd_phys = yd_phys;
        this.yd_abi = yd_abi;
    }

    public string ToString()
    {
        string res = "";
        
        res += origin.ToString();
        res += "\n";
        res += civilian.ToString();
        res += "\n";
        res += mission_command.ToString();
        res += "\n";
        res += enemy_desc.ToString();
        res += "\n";
        res += color.ToString();
        res += "\n";
        res += phys.ToString();
        res += "\n";
        res += star.ToString();
        res += "\n";
        res += color_shout.ToString();
        res += "\n";
        res += rd.ToString();
        res += rd_phys.ToString();
        res += rd_abi.ToString();
        res += "\n";
        res += rg.ToString();
        res += rg_phys.ToString();
        res += rg_abi.ToString();
        res += "\n";
        res += yd.ToString();
        res += yd_phys.ToString();
        res += yd_abi.ToString();
        
        
        return res;
    }
}

namespace GenerationScripts
{
    public class DescriptionFormatter
    {
        
        public string ReturnRandomDescription(Description desc, string match)
        {
            // create result string
            string result = "";
            // random init
            Random random = new Random();
            // bool to determine closed # brackets
            bool end = false;
            // int for index counting
            int start = 0;
            
            
            
            // switch case the match to find appropriate list to sample
            switch (match)
            {
                    case "origin":
                        result = desc.origin[random.Next(desc.origin.Count)];
                        break;
                    case "civilian":
                        result = desc.civilian[random.Next(desc.civilian.Count)];
                        break;
                    case "mission_command":
                        result = desc.mission_command[random.Next(desc.mission_command.Count)];
                        break;
                    case "enemy_desc":
                        result = desc.enemy_desc[random.Next(desc.enemy_desc.Count)];
                        break;
                    case "color":
                        result = desc.color[random.Next(desc.color.Count)];
                        break;
                    case "phys":
                        result = desc.phys[random.Next(desc.phys.Count)];
                        break;
                    case "star":
                        result = desc.star[random.Next(desc.star.Count)];
                        break;
                    case "color_shout":
                        result = desc.color_shout[random.Next(desc.color_shout.Count)];
                        break;
                    case "rd":
                        result = desc.rd[random.Next(desc.rd.Count)];
                        break;
                    case "rd_phys":
                        result = desc.rd_phys[random.Next(desc.rd_phys.Count)];
                        break;
                    case "rd_abi":
                        result = desc.rd_abi[random.Next(desc.rd_abi.Count)];
                        break;
                    case "rg":
                        result = desc.rg[random.Next(desc.rg.Count)];
                        break;
                    case "rg_phys":
                        result = desc.rg_phys[random.Next(desc.rg_phys.Count)];
                        break;
                    case "rg_abi":
                        result = desc.rg_abi[random.Next(desc.rg_abi.Count)];
                        break;
                    case "yd":
                        result = desc.yd[random.Next(desc.yd.Count)];
                        break;
                    case "yd_phys":
                        result = desc.yd_phys[random.Next(desc.yd_phys.Count)];
                        break;
                    case "yd_abi":
                        result = desc.yd_abi[random.Next(desc.yd_abi.Count)];
                        break;
                    
            }
            
            string resCase = "";
            // for every character in the line
            for (int x = 0; x<result.Length; x++)
            {
                // if first #, mark it
                if (result[x] == '#' && !end)
                {
                    end = true;
                    start = x;
                }
                // if second #, call recursion and insert at start
                else if(result[x] == '#' && end)
                {
                    end = false;
                    resCase = resCase.Remove(0, 1);
                    string temp = ReturnRandomDescription(desc, resCase);
                    
                    result = result.Remove(start, x - start+1);
                    result = result.Insert(start, temp);
                    x = 0;
                    continue;
                }
                
                // add onto match if open brackets
                if (end)
                {
                    resCase += result[x];
                }
            }
            
            return result;
        }
    }
}