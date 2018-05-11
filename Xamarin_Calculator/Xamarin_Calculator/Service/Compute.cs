using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Xamarin_Calculator.Service
{
    public class Compute:ICompute
    {

        float Computing(string input)
        {
            try
            {
                float _result = 0;
                Stack stack = new Stack();
                string[] _bag_for_input = input.Split(' ');
                foreach (var item in _bag_for_input)
                {
                    if (item == " ")
                    {

                    }
                    else if (Regex.IsMatch(item, "[0-9]"))
                    {
                        stack.Push(item);
                    }
                    else if (item == "+" || item == "*" || item == "-" || item == "/")
                    {
                        float _number2 = float.Parse(stack.Pop().ToString());
                        float _number1 = float.Parse(stack.Pop().ToString());
                        if (item == "+")
                        {
                            stack.Push(_number1 + _number2);
                        }
                        else if (item == "-")
                        {
                            stack.Push(_number1 - _number2);
                        }
                        else if (item == "*")
                        {
                            stack.Push(_number1 * _number2);
                        }
                        else if (item == "/")
                        {
                            stack.Push(_number1 / _number2);
                        }
                    }
                }
                _result = float.Parse(stack.Peek().ToString());
                return _result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public string Result(string input)
        {
            string temp = "";
            string output = "";
            Stack stack = new Stack();
            string result = "";
            string[] bag_for_input = Format(input).Split(' ');
            foreach (var item in bag_for_input)
            {
                if (String.IsNullOrWhiteSpace(item) || String.IsNullOrEmpty(item))
                {
                    continue;
                }
                else if (Regex.IsMatch(item, "[0-9]"))
                {
                    output = output + item + " ";
                }
                else if (item == "(")
                {
                    stack.Push("(");
                }
                else if (item == ")")
                {
                    while (stack.Peek().ToString() != "(")
                    {
                        output = output + stack.Pop().ToString() + " ";
                    }
                    if (stack.Peek().ToString() == "(")
                    {
                        stack.Pop();
                    }
                }
                else if (Check(item) > 0)
                {
                    if ((!(Regex.IsMatch(temp, "[0-9]"))) && (temp == "("))
                    {
                        output = output + "0 ";
                    }
                    if ((stack.Count > 0) && (Check(item) <= Check(stack.Peek().ToString())) && (Check(stack.Peek().ToString()) != -1) && (Check(stack.Peek().ToString()) != 0))
                    {
                        output = output + stack.Pop().ToString() + " ";
                        stack.Push(item);
                    }
                    else
                    {
                        stack.Push(item);
                    }
                }
                temp = item;
            }
            while (stack.Count != 0)
            {
                output = output + stack.Pop().ToString() + " ";
            }
            temp = null;

            result = Computing(output).ToString();
            return result;
        }
        /// <summary>
        /// thêm khoảng trắng vào giữa hai ký tự
        /// </summary>
        /// <param name="chuoi"></param>
        /// <returns></returns>
        string Format(string chuoi)
        {
            chuoi = Regex.Replace(chuoi, @"\+|\-|\*|\/|\%|\^|\)|\(", delegate (Match m)
            {
                return " " + m.Value.ToString() + " ";
            }
            );
            return chuoi;
        }
        /// <summary>
        /// Trả về 1 khi là dấu cộng hay trừ
        /// Trả về 3 khi là dấu nhân hay chia
        /// Trả về -1 khi là dấu đóng hay mở ngoặc
        /// Trả về 0 các trường hợp còn lại
        /// </summary>
        /// <param name="chuoi"></param>
        /// <returns></returns>
        int Check(string chuoi)
        {
            if (chuoi == "+" || chuoi == "-")
            {
                return 1;
            }
            else if (chuoi == "*" || chuoi == "/")
            {
                return 3;
            }
            else if (chuoi == ")" || chuoi == "(")
            {
                return -1;
            }
            return 0;

        }
    }

}
