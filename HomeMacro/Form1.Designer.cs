namespace HomeMacro
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MouseLocation = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.RGBDataLabel = new System.Windows.Forms.Label();
            this.SaveMousePoint = new System.Windows.Forms.Label();
            this.Detector = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.NextBtnLocation = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MouseLocation
            // 
            this.MouseLocation.AutoSize = true;
            this.MouseLocation.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MouseLocation.Location = new System.Drawing.Point(14, 9);
            this.MouseLocation.Name = "MouseLocation";
            this.MouseLocation.Size = new System.Drawing.Size(42, 64);
            this.MouseLocation.TabIndex = 1;
            this.MouseLocation.Text = "x :\r\ny :";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // RGBDataLabel
            // 
            this.RGBDataLabel.AutoSize = true;
            this.RGBDataLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RGBDataLabel.Location = new System.Drawing.Point(16, 332);
            this.RGBDataLabel.Name = "RGBDataLabel";
            this.RGBDataLabel.Size = new System.Drawing.Size(19, 63);
            this.RGBDataLabel.TabIndex = 2;
            this.RGBDataLabel.Text = "0\r\n0\r\n0";
            // 
            // SaveMousePoint
            // 
            this.SaveMousePoint.AutoSize = true;
            this.SaveMousePoint.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SaveMousePoint.Location = new System.Drawing.Point(16, 128);
            this.SaveMousePoint.Name = "SaveMousePoint";
            this.SaveMousePoint.Size = new System.Drawing.Size(62, 40);
            this.SaveMousePoint.TabIndex = 3;
            this.SaveMousePoint.Text = "save x :\r\nsave y :";
            // 
            // Detector
            // 
            this.Detector.AutoSize = true;
            this.Detector.Location = new System.Drawing.Point(496, 9);
            this.Detector.Name = "Detector";
            this.Detector.Size = new System.Drawing.Size(38, 12);
            this.Detector.TabIndex = 4;
            this.Detector.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "새로고침 버튼(R)";
            // 
            // NextBtnLocation
            // 
            this.NextBtnLocation.AutoSize = true;
            this.NextBtnLocation.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.NextBtnLocation.Location = new System.Drawing.Point(16, 223);
            this.NextBtnLocation.Name = "NextBtnLocation";
            this.NextBtnLocation.Size = new System.Drawing.Size(62, 40);
            this.NextBtnLocation.TabIndex = 6;
            this.NextBtnLocation.Text = "save x :\r\nsave y :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "다음 버튼(N)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "색상(C)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(364, 320);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 72);
            this.label4.TabIndex = 9;
            this.label4.Text = "키 설명\r\nS : 매크로 시작\r\nWin + F12 : 매크로 중지\r\nR : 새로고침 버튼의 위치 설정\r\nN : 다음단계 버튼의 위치 설정\r\nC :" +
    " 원하는 좌석의 색상 추출\r\n";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 404);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NextBtnLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Detector);
            this.Controls.Add(this.SaveMousePoint);
            this.Controls.Add(this.RGBDataLabel);
            this.Controls.Add(this.MouseLocation);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label MouseLocation;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label RGBDataLabel;
        private System.Windows.Forms.Label SaveMousePoint;
        private System.Windows.Forms.Label Detector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label NextBtnLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

