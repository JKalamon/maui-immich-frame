using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace SimpleImmichFrame.Behaviors
{
	public class BlurBehavior : Behavior<Image>
	{
		private Image _associatedObject;

		public static readonly BindableProperty BlurAmountProperty =
				BindableProperty.Create(
						propertyName: nameof(BlurAmount),
						returnType: typeof(float),
						declaringType: typeof(BlurBehavior),
						defaultValue: 10f,
						propertyChanged: OnBlurAmountChanged);

		public float BlurAmount
		{
			get => (float)GetValue(BlurAmountProperty);
			set => SetValue(BlurAmountProperty, value);
		}

		protected override void OnAttachedTo(Image bindable)
		{
			base.OnAttachedTo(bindable);
			_associatedObject = bindable;
			ApplyBlurEffect(bindable);
		}

		protected override void OnDetachingFrom(Image bindable)
		{
			base.OnDetachingFrom(bindable);
			_associatedObject = null;
		}

		private static void OnBlurAmountChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var behavior = (BlurBehavior)bindable;
			if (behavior._associatedObject != null)
			{
				behavior.ApplyBlurEffect(behavior._associatedObject);
			}
		}

		private void ApplyBlurEffect(Image image)
		{
#if IOS || MACCATALYST
			if (image.Handler?.PlatformView is UIKit.UIImageView imageView)
			{
				var effect = UIKit.UIBlurEffect.FromStyle(UIKit.UIBlurEffectStyle.Light);
				var blurView = new UIKit.UIVisualEffectView(effect);
				blurView.Frame = imageView.Bounds;
				blurView.AutoresizingMask = UIKit.UIViewAutoresizing.FlexibleWidth | UIKit.UIViewAutoresizing.FlexibleHeight;
				imageView.AddSubview(blurView);
			}
#elif ANDROID
                if (image.Handler?.PlatformView is Android.Widget.ImageView imageView)
                {
                    float radius = BlurAmount;
                    if (OperatingSystem.IsAndroidVersionAtLeast(31))
                    {
                        var effect = Android.Graphics.RenderEffect.CreateBlurEffect(radius, radius, Android.Graphics.Shader.TileMode.Decal);
                        imageView.SetRenderEffect(effect);
                    }
                    else
                    {
                        var renderScript = Android.Renderscripts.RenderScript.Create(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity);
                        var blurScript = Android.Renderscripts.ScriptIntrinsicBlur.Create(renderScript, Android.Renderscripts.Element.U8_4(renderScript));
                        
                        if (imageView.Drawable is Android.Graphics.Drawables.BitmapDrawable bitmapDrawable)
                        {
                            var bitmap = bitmapDrawable.Bitmap;
                            var input = Android.Renderscripts.Allocation.CreateFromBitmap(renderScript, bitmap);
                            var output = Android.Renderscripts.Allocation.CreateTyped(renderScript, input.Type);
                            
                            blurScript.SetRadius(radius);
                            blurScript.SetInput(input);
                            blurScript.ForEach(output);
                            
                            output.CopyTo(bitmap);
                            imageView.SetImageBitmap(bitmap);
                            
                            input.Destroy();
                            output.Destroy();
                            blurScript.Destroy();
                            renderScript.Destroy();
                        }
                    }
                }
#endif
		}
	}
}