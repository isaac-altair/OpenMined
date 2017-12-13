﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using OpenMined.Network.Controllers;
using OpenMined.Network.Utils;
using OpenMined.Syft.Tensor;

namespace OpenMined.Syft.Layer
{
    public class Model
    {
        
        protected static volatile int nCreated = 0;
        
        protected int id;
        public int Id => id;

        // indices for weights used in forward prediction (not inluding those in models array)
        protected List<int> parameters;
        
        // indices for models used in forward prediction (which themselves can contain weights)
        protected List<int> models;

        protected void init()
        {
            parameters = new List<int>();
            models = new List<int>();
        }
        
        protected virtual FloatTensor Forward(FloatTensor input)
        {
            // Model layer must implement forward
            throw new NotImplementedException();
        }

        public string ProcessMessage(Command msgObj, SyftController ctrl)
        {
            
            switch (msgObj.functionCall)
            {
                case "forward":
                {
                    var input = ctrl.getTensor(int.Parse(msgObj.tensorIndexParams[0]));
                    var result = this.Forward(input);
                    return result.Id + "";
                }
                case "params":
                {
                    string out_str = "";
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        if (i < parameters.Count - 1)
                        {
                            out_str += parameters[i].ToString() + ",";
                        }
                        else
                        {
                            out_str += parameters[i].ToString() + "";
                        }
                    }
                    return out_str;
                }
            }

            return ProcessMessageLocal(msgObj, ctrl);
        }

        public int[] GetParameters()
        {
            return parameters.ToArray();
        }

        public virtual string ProcessMessageLocal(Command msgObj, SyftController ctrl)
        {	
            return "Model.processMessage not Implemented:" + msgObj.functionCall;
        }

    }
}