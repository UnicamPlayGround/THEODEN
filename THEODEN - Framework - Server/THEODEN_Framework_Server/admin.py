from django.contrib import admin

# Register your models here.
from THEODEN_Framework_Server.models.Version import Version
from THEODEN_Framework_Server.models.asset_bundles.AssetBundles import AssetBundles
from THEODEN_Framework_Server.models.asset_bundles.Dependency import Dependency

admin.register(AssetBundles)
admin.register(Version)
admin.register(Dependency)
