using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ghost_Deploy;
using Siticone.Desktop.UI.WinForms;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using KeyAuth;
using DiscordMessenger;
using System.Reflection.Emit;
using System.Security.Principal;
using DiscordRPC;
using Microsoft.Win32;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using CustomControls;
using Ghost_DeploySmooth;
using System.Threading;

namespace Ghost_DeploySmooth
{
    public class SmoothTypeAnimation
    {
        [DllImport("user32.dll")]
        static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll")]
        static extern Point GetCaretPos(out Point point);

        private Point targetCaretPos;
        private Control textBoxControl;

        public SmoothTypeAnimation(Control textBoxControl)
        {
            this.textBoxControl = textBoxControl;
            textBoxControl.TextChanged += OnTextBoxTextChanged;

            // Inicializar a posição do cursor
            GetCaretPos(out targetCaretPos);

            // Inicializar e iniciar a thread de animação
            Thread animationThread = new Thread(AnimateCaret);
            animationThread.IsBackground = true;
            animationThread.Start();
        }

        private void OnTextBoxTextChanged(object sender, EventArgs e)
        {
            // Capturar a nova posição do cursor ao digitar
            Point temp;
            GetCaretPos(out temp);
            SetCaretPos(targetCaretPos.X, targetCaretPos.Y);
            targetCaretPos = temp;
        }

        private void AnimateCaret()
        {
            Point current = targetCaretPos;

            while (true)
            {
                if (current != targetCaretPos)
                {
                    if (Math.Abs(current.X - targetCaretPos.X) + Math.Abs(current.Y - targetCaretPos.Y) > 30)
                        current = targetCaretPos;
                    else
                    {
                        current.X += Math.Sign(targetCaretPos.X - current.X);
                        current.Y += Math.Sign(targetCaretPos.Y - current.Y);
                    }

                    // Invocar SetCaretPos na thread que criou o controle
                    textBoxControl.Invoke((Action)(() => SetCaretPos(current.X, current.Y)));
                }

                Thread.Sleep(3);
            }
        }
    }
}
