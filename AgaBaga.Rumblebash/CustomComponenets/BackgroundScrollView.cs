using System;
using System.Collections.Generic;
using System.Text;

namespace AgaBaga.Rumblebash.CustomComponenets
{
    [Register("Rumblebash.BackgroundScrollView")]
    public class BackgroundScrollView : HorizontalScrollView, Android.Views.View.IOnTouchListener
    {
        public BackgroundScrollView(Context context) : base(context) { }

        public BackgroundScrollView(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public BackgroundScrollView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

        public BackgroundScrollView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

        protected BackgroundScrollView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public bool OnTouch(View v, MotionEvent e)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return false;
        }
    }
}
